import * as zrender from 'zrender'
import {jsonData,anis} from '@/imageData/imgData.js'

class zrTool {

	constructor(domId, characterSize) {
		this.timer = null;
		this.zr = zrender.init(document.getElementById(domId));
		this.canvasSize = characterSize;
		this.imageGroup = [];

		this.playFrameCount = -1;

		this.frameGroup = [{
			frameIndex: 0,
			layers: [{
				zrImg: null,
				layer: 0
			}]
		}];

		this.initZr(domId);
	}

	initZr(domId) {
		let _this = this;
		_this.zr = zrender.init(document.getElementById(domId));
		_this.frameGroup = [];



	}

	initFrameGroup() {
		let _this = this;
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
	}

	clear() {
		this.zr.clear();
		this.initFrameGroup();
	}

	hideAll() {

		let _this = this;

		for (var i = 0; i < this.frameGroup.length; i++) {
			//console.log( zrTool.frameGroup[i])
			for (var j = 0; j < this.frameGroup[i].layers.length; j++) {
				if (this.frameGroup[i].layers[j].zrImg != null) {
					this.frameGroup[i].layers[j].zrImg.hide();
				}
			}
		}

	}

	play(fps) {
		let index = 0;
		let _this = this;

		if (_this.timer != null) {
			clearInterval(_this.timer);
		}

		let interval = 1000 / fps;

		_this.timer = setInterval(() => {

			_this.hideAll()

			for (var i = 0; i < _this.frameGroup[index].layers.length; i++) {
				if (_this.frameGroup[index].layers[i].zrImg != null)
					_this.frameGroup[index].layers[i].zrImg.show();
			}

			index++;

			if (index >= _this.playFrameCount) {
				index = 0;
			}
		}, interval)

	}

	async drawItem(type, ani, canvasdx, canvasdy) {
		let _this = this;
		let frameInfo = _this.findFrameInfo(type, ani);

		console.log(frameInfo);
		let sourceImage = await _this.loadImage(require("@/assets/" + frameInfo.imgUrl));

		for (var i = 0; i < frameInfo.frameCount; i++) {
			let img = _this.getZrImage(sourceImage, _this.canvasSize * i, frameInfo.rowIndex * _this.canvasSize, canvasdx, canvasdy);
			img.hide()
			_this.fillFrameImg(i, frameInfo.type, img, frameInfo.layer);
			_this.drawFrame(i);
		}
	}

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
	}
	findFrameInfo(type, aniName) {

		console.log(jsonData)
		let image = jsonData.data.filter(x => x.type == type)[0];
		let frameInfo = image.frameInfos.filter(x => x.aniName == aniName)[0];

		return {
			...frameInfo,
			...{
				layer: image.layer,
				type: image.type,
				imgUrl: image.imgUrl
			}
		}
	}
	getZrImage(sourceImage, imgdx, imgdy, canvasdx, canvasdy) {
		let _this = this;
		let offscreenCanvas = document.createElement('canvas');
		offscreenCanvas.width = _this.canvasSize;
		offscreenCanvas.height = _this.canvasSize;
		let ctx = offscreenCanvas.getContext('2d');
		ctx.imageSmoothingEnabled = false;

		ctx.drawImage(sourceImage, -imgdx, -imgdy, sourceImage.width * (_this.canvasSize / 64), sourceImage.height * (_this.canvasSize / 64));

		let img = new zrender.Image({
			style: {
				image: offscreenCanvas.toDataURL(), // 使用这个DataURL作为新图像
				x: canvasdx,
				y: canvasdy,
				width: _this.canvasSize,
				height: _this.canvasSize
			}
		})

		return img;
	}
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
	}
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