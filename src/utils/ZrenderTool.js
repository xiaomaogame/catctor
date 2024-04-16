import * as zrender from 'zrender'
import imgJsonData from '@/imageData/img.js'
const zrTool = {
	zr: null, //画布对象
	canvasSize: 64 * 5, //画布大小
	imageGroup: [],
	frameGroup: [{
		frameIndex: 0,
		layers: [{
			zrImg: null,
			layer: 0
		}]
	}],
	initZr(domId) {
		let _this = this;
		_this.zr = zrender.init(document.getElementById(domId));
		_this.frameGroup = [];


		for (var i = 0; i < 13; i++) {


			let layers = []; //每一帧的层数
			for (var j = -10; j <= 10; j++) {
				layers.push({
					zrImg: null,
					layer: j
				})
			}

			_this.frameGroup.push({
				frameIndex: i,
				layers: layers
			});
		}
	},
	clear() {
		this.zr.clear()
	},
	//增加帧内容
	fillFrameImg(frameIndex, type, zrImg, layer) {
		let frameItems = this.frameGroup.filter(item => {
			return item.frameIndex == frameIndex;
		});

		if (frameItems.length <= 0)
			return;

		let layers = frameItems[0].layers.filter(img => {
			return img.layer == layer;
		})

		if (layers.length <= 0)
			return;

		layers[0].zrImg = zrImg;
		layers[0].type = type;
	},
	findFrameInfo(type, aniName) {
		let image = imgJsonData.data.filter(x => x.type == type)[0];
		let frameInfo = image.frameInfos.filter(x => x.aniName == aniName)[0];

		return {
			...frameInfo,
			...{
				layer: image.layer,
				type: image.type,
				imgUrl: image.imgUrl
			}
		}
	},
	getZrImage(sourceImage, dx, dy) {
		let _this = this;
		let offscreenCanvas = document.createElement('canvas');
		offscreenCanvas.width = _this.canvasSize;
		offscreenCanvas.height = _this.canvasSize;
		let ctx = offscreenCanvas.getContext('2d');
		ctx.imageSmoothingEnabled = false;

		ctx.drawImage(sourceImage, -dx, -dy, sourceImage.width * (_this.canvasSize / 64), sourceImage.height * (_this.canvasSize / 64));

		let img = new zrender.Image({
			style: {
				image: offscreenCanvas.toDataURL(), // 使用这个DataURL作为新图像
				x: 0,
				y: 0,
				width: _this.canvasSize,
				height: _this.canvasSize
			}
		})

		return img;
	},
	drawFrame(frameindex = -1) {

		let _this = this;
		if (frameindex >= 0) {
			let frame = _this.frameGroup.filter(x => x.frameIndex == frameindex)[0];

			console.log("找到帧", frame)

			for (var i = 0; i < frame.layers.length; i++) {
				let item = frame.layers[i];

				if (item.zrImg != null) {
					console.log("画图")
					_this.zr.add(item.zrImg);
				}
			}
		}
	},
	loadImage(src) {
		return new Promise(function (resolve, reject) {
			var img = new Image();
			img.src = src;

			img.onload = function () {
				resolve(img);
			}
			img.onerror = function () {
				reject(new Error('Image load failed: ' + src));
			}

		});
	}
}

export default zrTool;