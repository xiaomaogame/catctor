<template>
	<div>
		<el-row>
			<el-col :span="16">
				<div class="uploadCanvasContainer">

					<div id="uploadCanvas"></div>
				</div>
			</el-col>
			<el-col :span="6">
				<div>
					<el-upload ref="upload" action="http://localhost:21422/api/character/uploadImage" :data="form"
						:on-change="handleUploadChange" :file-list="fileList" drag :auto-upload="false"
						:on-success="onSuccess">
						<!-- 	<el-button slot="trigger" size="small" type="primary">选择图片</el-button> -->
						<i class="el-icon-upload"></i>
						<div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
					</el-upload>
				</div>
				<div style="margin-top: 20px;">
					<!-- <div id="iconCanvas" style="width:64px;height:64px;"></div> -->

					<el-form ref="form" :model="form" label-width="150px">
						<el-form-item label="选择图标:">
							<div style="display: flex;justify-content: flex-start;">
								<div id="iconCanvas" style="width:64px;height:64px;"></div>
								<div style="width: 150px;margin-left: 20px;">
									<span class="demonstration">缩放</span>
									<el-slider @input="iconScaleInputHandler" v-model="iconScaleValue" :min="1" :max="3"
										:step="0.5" show-stops>
									</el-slider>
								</div>
							</div>
						</el-form-item>
						<el-form-item label="类型:">
							<el-select v-model="form.code" @change="typeSelectHandler">
								<el-option v-for="item in typeListOptions" :value="item.code" :label="item.name">

								</el-option>
							</el-select>
						</el-form-item>
						<el-form-item label="性别:">
							<el-radio v-model="form.sex" label="男">男</el-radio>
							<el-radio v-model="form.sex" label="女">女</el-radio>
							<el-radio v-model="form.sex" label="无">无</el-radio>
						</el-form-item>
						<el-form-item label="多图层:">
							<el-radio v-model="form.pos" label="前">前</el-radio>
							<el-radio v-model="form.pos" label="后">后</el-radio>
						</el-form-item>
						<el-form-item :label="imgName">
							<el-input v-model="form.name"></el-input>
						</el-form-item>
						<el-form-item label="描述">
							<el-input v-model="form.desc"></el-input>
						</el-form-item>

						<el-button style="margin-top: 10px; float: right;" size="small" type="success"
							@click="handleUpload">上传图片</el-button>
					</el-form>
				</div>
			</el-col>
		</el-row>


	</div>
</template>

<script>
	import * as zrender from 'zrender'
	import {
		MessageBox,
		Message
	} from 'element-ui'
	import {
		GetImgTablesPost,
		GetNamePost
	} from '@/api/myApp'

	export default {
		data() {
			return {
				zr: null,
				fileList: [],
				currentFileName: "",
				currentImg: null,
				iconScaleValue: 1,
				currentIconPos: {
					x: 0,
					y: 0
				},
				form: {
					code: "",
					name: "",
					sex: "男",
					iconData: "",
					pos: "前",
					desc: ""
				},
				typeListOptions: []
			};
		},
		async mounted() {
			let _this = this;
			this.zr = zrender.init(document.getElementById("uploadCanvas"));
			this.iconZr = zrender.init(document.getElementById("iconCanvas"));

			await this.initTypeList()
			this.zr.on('click', function(e) {

				// if (!this.currentImg)
				// 	return;

				//获取点击的坐标
				var x = e.offsetX;
				var y = e.offsetY;

				//计算64*64区域的左上角坐标
				var leftTopX = Math.floor(x / 64) * 64
				var leftTopY = Math.floor(y / 64) * 64


				_this.currentIconPos.x = leftTopX;
				_this.currentIconPos.y = leftTopY;


				console.log("drawicon3")
				_this.drawIcon()


			});
		},
		computed: {
			imgName() {
				if (this.form.code == "")
					return "图片名称:"
				else
					return "图片名称: " + this.form.code + "_"
			}
		},
		methods: {
			async initTypeList() {
				let res = await GetImgTablesPost({});
				this.typeListOptions = res.data;
			},
			drawIcon() {
				let scale = this.iconScaleValue
				let _this = this;

				if (!_this.currentImg)
					return;

				let offscreenCanvas = document.createElement('canvas');
				offscreenCanvas.width = 64 * scale;
				offscreenCanvas.height = 64 * scale;
				let ctx = offscreenCanvas.getContext('2d');
				ctx.imageSmoothingEnabled = false;

				ctx.drawImage(_this.currentImg, -_this.currentIconPos.x * scale, -_this.currentIconPos.y * scale, _this
					.currentImg.width *
					scale, _this
					.currentImg.height * scale);

				let imageData = ctx.getImageData(0, 0, 64 * scale, 64 * scale);
				var pixels = imageData.data;

				var minX = imageData.width;
				var minY = imageData.height;
				var maxX = -1;
				var maxY = -1;

				for (var y = 0; y < imageData.height; y++) {
					for (var x = 0; x < imageData.width; x++) {
						var index = (y * imageData.width + x) * 4;
						var alpha = pixels[index + 3];
						if (alpha !== 0) { // 如果像素不透明
							minX = Math.min(minX, x);
							minY = Math.min(minY, y);
							maxX = Math.max(maxX, x);
							maxY = Math.max(maxY, y);
						}
					}
				}

				let iconPX = 0;
				let iconPY = 0;

				if (maxX < minX || maxY < minY) {
					console.log('The image is fully transparent.');
				} else {
					var width = maxX - minX + 1;
					var height = maxY - minY + 1;
					console.log('The actual size of image content is', width, 'x', height +
						', top-left corner at (' + minX + ', ' + minY + ')');

					iconPX = minX + width / 2 - 32;
					iconPY = minY + height / 2 - 32;


				}


				let img = new zrender.Image({
					style: {
						image: offscreenCanvas.toDataURL(),
						width: 64 * scale,
						height: 64 * scale,
						x: -iconPX,
						y: -iconPY
					}
				})
				_this.iconZr.clear();
				_this.iconZr.add(img);
			},
			iconScaleInputHandler(val) {
				console.log("drawicon1")
				this.drawIcon()
			},
			handleUploadChange(file, fileList) {


				console.log(file)
				console.log(fileList)
				let _this = this;
				_this.fileList = fileList.slice(-1); // Always keep the last selected file

				let image = new Image();
				image.onload = () => {
					_this.zr.clear();
					let img = new zrender.Image({
						style: {
							image: image,
							width: image.width,
							height: image.height,
							x: 0,
							y: 0
						}
					})
					_this.currentImg = image;

					_this.zr.add(img);


					console.log("drawicon2")
					_this.drawIcon();


					if (this.form.code != "") {
						GetNamePost({
							code: _this.form.code
						}).then(res => {
							if (_this.fileList.length > 0)
								_this.form.name = _this.fileList[0].name.replace(".png", "") + "_" + res.data
							else
								_this.form.name = res.data;


							if (_this.fileList[0].name.indexOf("female") > -1) {
								_this.form.sex = '女';
							} else if (_this.fileList[0].name.indexOf("male") > -1) {
								_this.form.sex = '男';
							} else {
								_this.form.sex = '无';
							}
						})
					}



				}
				image.src = URL.createObjectURL(file.raw);

			},
			typeSelectHandler(val) {
				let _this = this;
				GetNamePost({
					code: val
				}).then(res => {
					if (_this.fileList.length > 0)
						_this.form.name = _this.fileList[0].name.replace(".png", "") + "_" + res.data
					else
						_this.form.name = res.data;

					if (_this.fileList[0].name.indexOf("female") > -1) {
						_this.form.sex = '女';
					} else if (_this.fileList[0].name.indexOf("male") > -1) {
						_this.form.sex = '男';
					} else {
						_this.form.sex = '无';
					}
				})
			},
			handleUpload() {
				// 获取 div 元素
				let div = document.getElementById('iconCanvas');

				// 获取 div 元素下的第一个 canvas 元素
				let canvas = div.getElementsByTagName('canvas')[0];

				let base64 = canvas.toDataURL();
				this.form.iconData = base64;
				this.$refs.upload.submit();
				console.log("上传")
			},
			onSuccess(res) {

				console.log(res)
				if (res.code == 20000) {
					Message({
						message: '上传成功!',
						type: 'success',
						duration: 5 * 1000
					})
				}

				if (res.code == 40000) {
					Message({
						message: res.message,
						type: 'error',
						duration: 5 * 1000
					})
				}

			}
		}
	};
</script>

<style type="text/css">
	.uploadLeft {
		display: flex;
		flex-direction: column;
		align-content: space-between;
		flex-wrap: wrap;
		align-items: flex-start;
		justify-content: center;
	}

	.uploadCanvasContainer {
		height: 80vh;
		width: 850px;
		overflow-y: auto;
		border: 1px solid #ccc;
	}

	#uploadCanvas {
		width: 832px;
		height: 1344px;
	}

	#iconCanvas {
		width: 64px;
		height: 64px;
		border: 1px solid #ccc;
	}

	/* 自定义滚动条的样式 */
	::-webkit-scrollbar {
		width: 10px;
	}

	/* 底色 */
	::-webkit-scrollbar-track {
		background: #f1f1f1;
	}

	/* 滑块样式 */
	::-webkit-scrollbar-thumb {
		background: #888;
		border-radius: 10px;
	}

	/* 滑块被鼠标选中时的样式 */
	::-webkit-scrollbar-thumb:hover {
		background: #555;
	}
</style>