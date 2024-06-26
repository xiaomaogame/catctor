import * as zrender from 'zrender'

import {
	GetAniInfo,
	GetImgData
} from '@/imageData/imgData'
import imageHelper from '@/utils/imageHelper'

import ZRImage from 'zrender/lib/graphic/Image';
import Layer from 'zrender/lib/canvas/Layer';
import CharacterData from "@/imageData/characterData"

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

		let charData = new CharacterData();
	
		this.anis = charData.anis;
		this.jsonData = charData.jsonData;

		this.initZr(domId);
	}

	initZr(domId) {
		let _this = this;
		_this.zr = zrender.init(document.getElementById(domId));
		_this.frameGroup = [];
		_this.initFrameGroup();
	}

	initFrameGroup() {
		let _this = this;
		_this.frameGroup = [];

		for (var i = 0; i < 13; i++) {


			let layers = []; //每一帧的层数
			for (var j = -30; j <= 30; j++) {
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

	async drawItem({
		imageInfo,
		imgType,
		aniName,
		canvasdx,
		canvasdy,
		colorFunc
	}) {

	
		if (!canvasdx)
			canvasdx = 0;

		if (!canvasdy)
			canvasdy = 0;

		let _this = this;
		//图片信息，图片地址和绘制层级

		if (!imageInfo)
			return;

		//动画信息，序列帧位置
		let aniInfo = _this.findAniInfo(aniName);

		if (!aniInfo)
			return;
		//整合后的序列帧位置
		let framePos = imageHelper.getFramePos(aniInfo.framePos);

		//原始图片
		let sourceImage = imageInfo.canvas;
		//let sourceImage = await _this.loadImage(imageInfo.dataUrl);

		//循环绘制序列帧
		for (var i = 0; i < framePos.length; i++) {
			// let img = _this.getZrImage(sourceImage, framePos[i][1] * _this.canvasSize, framePos[i][0] * _this
			// 	.canvasSize, canvasdx, canvasdy);

			//获取ZrImage
			let img = _this.getZrImage({
				sourceImage: sourceImage,
				imgdx: framePos[i][1] * _this.canvasSize,
				imgdy: framePos[i][0] * _this.canvasSize,
				canvasdx: canvasdx,
				canvasdy: canvasdy,
				layer: imageInfo.layer
			});

			//img.hide()
			//填充frameGroup begin -----------------------
			let frameItems = _this.frameGroup.filter(item => {
				return item.frameIndex == i;
			});

			if (frameItems.length <= 0)
				return;

			let layers = frameItems[0].layers.filter(item => {
				return item.layer == imageInfo.layer;
			})


			if (layers.length <= 0)
				return;

			if (layers[0].zrImg != null) {
				layers[0].zrImg.attr({
					style: {
						image: img.canvas.toDataURL()
					}
				})
				layers[0].zrImg.canvas = img.canvas;
				layers[0].zrImg.type = imageInfo.type;
			} else {
				layers[0].zrImg = img;
				layers[0].type = imageInfo.type;
				_this.zr.add(img);
			}

			//填充frameGroup end ------------------------------
		}
	}
	findImgInfo(type) {
		if (type == "")
			return null;
		let code = type.split('_')[0];
		let imageInfo = this.jsonData.filter(x => x.code == code)[0];
		let layer = imageInfo.layer;
		let image = imageInfo.imgs.filter(x => x.type == type)[0];
		image.layer = layer;
		return image;
	}

	findAniInfo(aniName) {
		let aniInfo = this.anis.filter(x => x.aniName == aniName)[0];
		return aniInfo;
	}
	getZrImage({
		sourceImage,
		imgdx,
		imgdy,
		canvasdx,
		canvasdy,
		layer,
		colorFunc
	}) {


		if (!imgdx)
			imgdx = 0;
		if (!imgdy)
			imgdy = 0;

		let _this = this;
		let offscreenCanvas = document.createElement('canvas');
		offscreenCanvas.width = _this.canvasSize;
		offscreenCanvas.height = _this.canvasSize;
		let ctx = offscreenCanvas.getContext('2d');
		ctx.imageSmoothingEnabled = false;

		ctx.drawImage(sourceImage, -imgdx, -imgdy, sourceImage.width * (_this.canvasSize / 64), sourceImage.height *
			(_this.canvasSize / 64));

		if (colorFunc != null) {
			colorFunc(ctx);
		}

		let img = new zrender.Image({
			style: {
				image: offscreenCanvas.toDataURL(), // 使用这个DataURL作为新图像
				x: canvasdx,
				y: canvasdy,
				width: _this.canvasSize,
				height: _this.canvasSize
			},
			zlevel: layer
		});

		img.canvas = offscreenCanvas;
		return img;
	}
	// drawFrame(frameindex = -1) {

	// 	let _this = this;
	// 	if (frameindex >= 0) {
	// 		let frame = _this.frameGroup.filter(x => x.frameIndex == frameindex)[0];

	// 		console.log("找到帧", frame)

	// 		for (var i = 0; i < frame.layers.length; i++) {
	// 			let item = frame.layers[i];

	// 			if (item.zrImg != null) {
	// 				console.log("画图")
	// 				_this.zr.add(item.zrImg);
	// 			}
	// 		}
	// 	}
	// }
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
	}
}

export default zrTool;