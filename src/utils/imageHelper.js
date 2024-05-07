import * as zrender from 'zrender'
const imageHelper = {
	rgbaToHsl(r, g, b, a) {
		r /= 255, g /= 255, b /= 255;
		let max = Math.max(r, g, b),
			min = Math.min(r, g, b);
		let h, s, l = (max + min) / 2;

		if (max == min) {
			h = s = 0; // achromatic
		} else {
			let d = max - min;
			s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
			switch (max) {
				case r:
					h = (g - b) / d + (g < b ? 6 : 0);
					break;
				case g:
					h = (b - r) / d + 2;
					break;
				case b:
					h = (r - g) / d + 4;
					break;
			}
			h /= 6;
		}
		return [h * 360, s, l, a];
	},
	loadImage(src) {
		return new Promise(function(resolve, reject) {
			var img = new Image();
			img.src = src;

			img.onload = function() {
				resolve(img);
			}
			img.onerror = function() {
				reject(new Error('Image load failed: ' + src));
			}

			img.crossOrigin = "anonymous";

		});
	},
	getFramePos(framePos) {
		let tempPos = [];

		for (let index = 0; index < framePos.length; index++) {
			let element = framePos[index];
			if (element.length == 3) {
				for (let i = element[1]; i < element[2]; i++) {
					tempPos.push([element[0], i]);
				}
			} else {
				tempPos.push(element)
			}
		}

		return tempPos;
	},
	changeHSL(hsl, canvas) {
		let ctx = canvas.getContext('2d');
		ctx.imageSmoothingEnabled = false;

		let rgbaArr = [];

		let imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
		let data = imageData.data;
		for (let i = 0; i < data.length; i += 4) {
			let r = data[i];
			let g = data[i + 1];
			let b = data[i + 2];
			let a = data[i + 3];

			let rgba = `rgba(${r},${g},${b},${a})`;
			let ohsl = this.rgbaToHsl(r, g, b, a);

			let newrgbaStr = zrender.color.modifyHSL(rgba, hsl.h, hsl.s, ohsl[2] + hsl.l);

			let res = /rgba\((\d+),\s*(\d+),\s*(\d+),\s*(\d*\.*\d*)\)/.exec(newrgbaStr);
			let colorData = {
				r: parseInt(res[1], 10),
				g: parseInt(res[2], 10),
				b: parseInt(res[3], 10),
				a: parseFloat(res[4])
			};

			data[i] = colorData.r;
			data[i + 1] = colorData.g;
			data[i + 2] = colorData.b;
		}

		ctx.putImageData(imageData, 0, 0);
	}
}

export default imageHelper