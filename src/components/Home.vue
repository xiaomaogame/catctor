<template>
	<div>
		<el-row>
			<el-col :span="6">
				<div style="margin-right: 10px;">
					<el-card class="box-card">
						<div style="height: 80vh;overflow: auto;">
							<el-collapse>
								<el-collapse-item v-for="iconItem in jsonData">
									<template slot="title" @click.stop>
										<el-checkbox :value="iconActive[iconItem.code] != '' ? true : false"
											@click.stop.native="() => { }"
											@change="(checked) => iconCheckHandler(checked, iconItem.code)">{{
												iconItem.name + ' (' + iconItem.imgs.filter(x=>x.pos!='后').length + ') ' }}
											<el-button size="mini" v-if="iconActive[iconItem.code] != ''"
												@click="colorPanelShow(iconItem.code, iconItem.name)"
												round>自定义颜色</el-button>
										</el-checkbox>

									</template>
									<div class="iconContainer">
										<div :class="iconActive[iconItem.code] == icon.type ? 'iconBoxActive' : ''"
											v-for="icon in iconItem.imgs">
											<div class="iconBox" v-if="icon.pos!='后'">
												<el-image style="width: 64px; height: 64px" :src="icon.iconData"
													fit="fill" @click="choseIconHandler(iconItem.code, icon.type)"
													@contextmenu.prevent="rightClick(icon)"></el-image>
												<i v-if="icon.sex == '男'" class="el-icon-male"
													style="color: blue;position: absolute;right: 0;bottom: 0;"></i>
												<i v-if="icon.sex == '女'" class="el-icon-female"
													style="color: hotpink;position: absolute;right: 0;bottom: 0;"></i>

												<i v-if="icon.afterId>0" class="el-icon-connection"
													style="color: darkgreen;position: absolute;left:0;bottom: 0;"></i>
											</div>

										</div>

									</div>
								</el-collapse-item>
							</el-collapse>
						</div>
					</el-card>

				</div>
			</el-col>
			<el-col :span="12">
				<el-card class="box-card" v-loading="loading" element-loading-text="拼命加载中"
					element-loading-background="rgba(0, 0, 0, 0.8)">
					<div class="mainPage">
						<div class="canvasContainer" style="margin-bottom: 20px;">
							<div class="aniBox">
								<el-radio v-model="aniRadio" @input="aniSelectHandler" label="walk">走路</el-radio>
								<el-radio v-model="aniRadio" @input="aniSelectHandler" label="fashu">施法</el-radio>
								<el-radio v-model="aniRadio" @input="aniSelectHandler" label="tuci">突刺</el-radio>
							</div>
							<div id="mainCanvas"></div>
							<div class="aniBox">
								<el-radio v-model="aniRadio" @input="aniSelectHandler" label="fangyu">防御</el-radio>
								<el-radio v-model="aniRadio" @input="aniSelectHandler" label="shejian">射箭</el-radio>
								<el-radio v-model="aniRadio" @input="aniSelectHandler" label="die">死亡</el-radio>
							</div>
						</div>
						<div class="subCanvasContainer" style="margin-bottom: 20px;">
							<div class="subCanvas" id="upCanvas"></div>
							<div class="subCanvas" id="leftCanvas"></div>
							<div class="subCanvas" id="downCanvas"></div>
							<div class="subCanvas" id="rightCanvas"></div>
						</div>
						<div>
							<el-form ref="form" :model="form" label-width="100px">
								<el-form-item label="速度(FPS)">
									<el-slider v-model="form.fps" :min="1" :max="30" @change="speedChange"></el-slider>
								</el-form-item>
							</el-form>
						</div>
					</div>

				</el-card>
			</el-col>
			<el-col :span="6">
				<!-- <div v-for="(item,index) in currentSelectType">
					{{getCodeName(item)}}

				</div> -->
				<div style="margin-left: 10px;">
					<el-card class="box-card">
						<div>
							<el-radio v-model="exportType" label="big">单张大图</el-radio>
							<el-radio v-model="exportType" label="sub">分帧压缩包</el-radio>
						</div>
						<div style="margin-top: 20px;">
							<el-button @click="download" :disabled="exportDisabled">导出</el-button>
						</div>
					</el-card>
					<div style="margin-top: 10px;">
						<el-card class="box-card">
							<p>作者：小猫学游戏</p>
							<a href="https://space.bilibili.com/627968233" target="_blank">B站主页</a>
							<p>版本：1.0</p>
							<p>服务器到期时间：2025-04-30</p>
							<p>你可以在以下网站找到有关LPC项目的更多信息:</p>
							<ul>
								<li><a href="https://lpc.opengameart.org/" rel="nofollow noopener"
										referrerpolicy="origin">About the Liberated Pixel Cup</a></li>
								<li><a href="https://lpc.opengameart.org/static/LPC-Style-Guide/build/index.html"
										rel="nofollow noopener" referrerpolicy="origin">Liberated Pixel Cup style guide,
										assets, and demo</a></li>
								<li><a href="https://bztsrc.gitlab.io/lpc-refined/" rel="nofollow noopener"
										referrerpolicy="origin">Liberated Pixel Cup Specification</a></li>
								<li><a href="https://opengameart.org/" rel="nofollow noopener"
										referrerpolicy="origin">OpenGameArt</a></li>
								<li><a href="https://sanderfrenken.github.io/Universal-LPC-Spritesheet-Character-Generator/"
										rel="nofollow noopener" referrerpolicy="origin">Universal LPC Spritesheet
										Generator</a></li>
							</ul>
							<p>以下是资源许可证信息</p>
							
							<el-input type="textarea" :rows="3" v-model="textarea">
							</el-input>
						</el-card>
					</div>
					

				</div>
			</el-col>

		</el-row>

		<el-dialog title="修改" :visible.sync="dialogFormVisible" width="30%">
			<el-form :model="iconEditform" label-width="100px">
				<el-form-item label="类型">
					<el-select style="width: 100%;" v-model="iconEditform.code" placeholder="请选择类型">
						<el-option v-for="item in typeListOptions" :value="item.code" :label="item.name">

						</el-option>
					</el-select>
				</el-form-item>
				<el-form-item label="名称(唯一)">
					<el-input v-model="iconEditform.type" autocomplete="off"></el-input>
				</el-form-item>
				<el-form-item label="性别:">
					<el-radio v-model="iconEditform.sex" label="男">男</el-radio>
					<el-radio v-model="iconEditform.sex" label="女">女</el-radio>
					<el-radio v-model="iconEditform.sex" label="无">无</el-radio>
				</el-form-item>

				<el-form-item label="描述">
					<el-input v-model="iconEditform.desc" autocomplete="off"></el-input>
				</el-form-item>
			</el-form>
			<div slot="footer" class="dialog-footer">
				<el-button type="danger" @click="delImgJsonHandler">删 除</el-button>
				<el-button @click="dialogFormVisible = false">取 消</el-button>
				<el-button type="primary" @click="editIconHandler">确 定</el-button>
			</div>
		</el-dialog>


		<div class="colorDialogContainer">
			<el-dialog :title="'自定义颜色_' + currentColorCodeName" :visible.sync="colorDialogFormVisible" width="30%"
				:modal="false" :close-on-click-modal="false">

				<el-form :model="colorCtrl" label-width="10px">
					<div>
						<el-form-item label="">
							<el-checkbox v-model="colorCtrl.hchecked">色相</el-checkbox>
							<el-slider v-model="colorCtrl.h" :max="360" class="sexiang"></el-slider>
						</el-form-item>
					</div>
					<el-form-item label="">
						<el-checkbox v-model="colorCtrl.schecked">饱和度</el-checkbox>
						<el-slider v-model="colorCtrl.s" :max="100"></el-slider>
					</el-form-item>

					<el-form-item label="">
						<el-checkbox v-model="colorCtrl.lchecked">明度</el-checkbox>
						<el-input-number v-model="colorCtrl.l" :min="-100" :max="100" label="描述文字"></el-input-number>
					</el-form-item>
				</el-form>

				<div slot="footer" class="dialog-footer">
					<el-button @click="colorDialogFormVisible = false">取 消</el-button>
					<el-button type="primary" @click="colorChangeHandler()">确 定</el-button>
				</div>
			</el-dialog>
		</div>
	</div>

</template>

<script>
	import zrTool from "@/utils/ZrenderTool"
	import CharacterData from "@/imageData/characterData"
	import * as zrender from 'zrender'

	import {
		GetImgTablesPost,
		EditIconPost,
		DelImgJsonPost,
		ExportImgPackagePost,
		DownloadFile,
		GetCredits
	} from '@/api/myApp'

	import imageHelper from '@/utils/imageHelper'

	import {
		MessageBox,
		Message
	} from 'element-ui'
	import Layer from "zrender/lib/canvas/Layer"

	export default {
		name: "Home",
		data() {
			return {
				dialogFormVisible: false, //编辑框
				colorDialogFormVisible: false, //颜色编辑框
				exportType: "big", //导出模式
				exportDisabled: false, //是否允许导出
				textarea: "", //协议信息
				anis: [], //动画信息
				mainZr: null, //主图Zrender
				upZr: null,
				leftZr: null,
				downZr: null,
				rightZr: null,
				loading: false, //加载框
				timer: null, //计时器
				farmeCount: 0, //当前动画帧数量
				currentColorCodeName: "", //当前改变颜色的项目名称
				form: {
					fps: 12 //帧率
				},
				iconEditform: { //图标编辑信息
					id: "",
					code: "",
					desc: "",
					type: "",
					sex: ""
				},
				typeListOptions: [], //所有项目的类型列表
				aniRadio: "walk", //动画选项
				iconDefaultType: { //默认选项

				},
				iconActive: { //当前选项

				},
				jsonData: [], //所有图像信息
				currentSelectType: ["body_male"], //当前勾选的类型
				colorCtrl: { //颜色控制
					h: 0,
					hchecked: false,
					s: 100,
					schecked: false,
					l: 0,
					lchecked: false,
					code: ""
				},
				imgCnavasList: [{ //图像canvas的实例
					code: "",
					imgType: "",
					canvas: null,
					dataUrl: "",
					layer: ""
				}]
			}
		},
		async mounted() {

			this.loading = true;
			await this.init();
			this.loading = false;
		},
		methods: {
			async init() {
				//初始化类型
				await this.initTypeList();
				//初始化证书
				this.initCredits();
				//获取单例基础数据
				let characterData = new CharacterData();
				await characterData.init();
				this.anis = characterData.anis;
				this.jsonData = characterData.jsonData;

				//初始化主图和四个子图
				this.initZr();


				//初始化图层信息
				await this.initSelectInfo();

				//渲染
				this.renderAnimation(this.currentSelectType, this.aniRadio)
				//播放
				this.playAnimation();
			},
			initZr() {
				this.mainZr = new zrTool("mainCanvas", 320);

				let size = 128;
				this.upZr = new zrTool("upCanvas", size);
				this.leftZr = new zrTool("leftCanvas", size);
				this.downZr = new zrTool("downCanvas", size);
				this.rightZr = new zrTool("rightCanvas", size);


				for (let key in this.iconDefaultType) {
					this.iconActive[key] = this.iconDefaultType[key];
				}
			},
			initCredits() {
				let _this = this;
				GetCredits().then(res => {
					_this.textarea = res.data;
				});
			},
			async initSelectInfo() {

				let _this = this;

				let selectItemType = [];
				for (let key in _this.iconActive) {
					if (_this.iconActive[key] != "")
						selectItemType.push(_this.iconActive[key])
				}

				_this.currentSelectType = selectItemType;



				for (var i = 0; i < _this.currentSelectType.length; i++) {
					let type = _this.currentSelectType[i];
					let code = type.split("_")[0];


					let itemInfo = _this.jsonData.filter(x => x.code == code)[0];
					let imgInfo = itemInfo.imgs.filter(x => x.type == type && x.pos != '后')[0];
					imgInfo.layer = itemInfo.layer;

					let imgCanvasInfo = _this.imgCnavasList.filter(x => x.code == code)[0];

					//已经存在一模一样的图层，避免颜色被刷新掉
					if (imgCanvasInfo && imgCanvasInfo.type == type) {

						continue;
					}

					//构造canvas
					let sourceImage = await imageHelper.loadImage(process.env.VUE_APP_BASE_API + "/resources/" +
						imgInfo.imgUrl);

					let offscreenCanvas = document.createElement('canvas');
					offscreenCanvas.width = sourceImage.width;
					offscreenCanvas.height = sourceImage.height;
					let ctx = offscreenCanvas.getContext('2d');
					ctx.imageSmoothingEnabled = false;

					ctx.drawImage(sourceImage, 0, 0, sourceImage.width, sourceImage.height);

					imgInfo.canvas = offscreenCanvas;
					// imgInfo.dataUrl = offscreenCanvas.toDataURL()


					// console.log(imgInfo)
					if (imgInfo.afterId > 0) {
						//找后一个图
						let afterinfo = itemInfo.imgs.filter(x => x.pos == '后')[0];
						//console.log("afterinfo",afterinfo)
						//构造canvas
						let aftersourceImage = await imageHelper.loadImage(process.env.VUE_APP_BASE_API +
							"/resources/" +
							afterinfo.imgUrl);

						let afteroffscreenCanvas = document.createElement('canvas');
						afteroffscreenCanvas.width = sourceImage.width;
						afteroffscreenCanvas.height = sourceImage.height;
						let afterctx = afteroffscreenCanvas.getContext('2d');
						afterctx.imageSmoothingEnabled = false;

						afterctx.drawImage(aftersourceImage, 0, 0, aftersourceImage.width, aftersourceImage.height);

						afterinfo.canvas = afteroffscreenCanvas;
						afterinfo.layer = -itemInfo.layer;

						imgInfo.afterInfo = afterinfo;

					}
					console.log(imgInfo)

					if (imgCanvasInfo) {
						_this.imgCnavasList = _this.imgCnavasList.filter(x => x.code != code);
					}
					_this.imgCnavasList.push(imgInfo);
				}

			},
			async initTypeList() {
				let res = await GetImgTablesPost({});
				this.typeListOptions = res.data;

				for (var i = 0; i < this.typeListOptions.length; i++) {
					this.iconDefaultType[this.typeListOptions[i].code] = "";
					this.iconActive[this.typeListOptions[i].code] = "";
				}

				this.iconDefaultType.body = "body_male";
				this.iconDefaultType.head = "head_male"

				this.iconDefaultType = {
					...this.iconDefaultType
				}
				this.iconActive = {
					...this.iconActive
				}
			},
			colorPanelShow(code, name) {
				this.currentColorCodeName = name;
				this.colorCtrl.h = 0;
				this.colorCtrl.s = 100;
				this.colorCtrl.l = 0;
				this.colorCtrl.code = code;
				this.colorDialogFormVisible = true;
			},
			colorChangeHandler() {

				let _this = this;

				if (!_this.colorCtrl.hchecked && !_this.colorCtrl.schecked && !_this.colorCtrl.lchecked)
					return;

				let hsl = {
					h: _this.colorCtrl.hchecked ? _this.colorCtrl.h : null,
					s: _this.colorCtrl.schecked ? (_this.colorCtrl.s / 100) : null,
					l: _this.colorCtrl.lchecked ? (_this.colorCtrl.l / 100) : null
				}

				let imgCanvasItem = _this.imgCnavasList.filter(x => x.code == this.colorCtrl.code)[0];

				if (imgCanvasItem) {
					imageHelper.changeHSL(hsl, imgCanvasItem.canvas);
				}

				if (imgCanvasItem.afterInfo) {
					imageHelper.changeHSL(hsl, imgCanvasItem.afterInfo.canvas);
				}


				this.renderAnimation(this.currentSelectType, this.aniRadio);

				// for (var i = 0; i < _this.imgCnavasList.length; i++) {
				// 	let item = _this.imgCnavasList[i];
				// 	if (item.code == this.colorCtrl.code) {
				// 		let ctx = item.canvas.getContext('2d');
				// 		ctx.imageSmoothingEnabled = false;

				// 		let rgbaArr = [];

				// 		let imageData = ctx.getImageData(0, 0, item.canvas.width, item.canvas.height);
				// 		let data = imageData.data;
				// 		for (let i = 0; i < data.length; i += 4) {
				// 			let r = data[i];
				// 			let g = data[i + 1];
				// 			let b = data[i + 2];
				// 			let a = data[i + 3];

				// 			let rgba = `rgba(${r},${g},${b},${a})`;
				// 			let ohsl = imageHelper.rgbaToHsl(r, g, b, a);

				// 			let newrgbaStr = zrender.color.modifyHSL(rgba, hsl.h, hsl.s, ohsl[2] + hsl.l);

				// 			let res = /rgba\((\d+),\s*(\d+),\s*(\d+),\s*(\d*\.*\d*)\)/.exec(newrgbaStr);
				// 			let colorData = {
				// 				r: parseInt(res[1], 10),
				// 				g: parseInt(res[2], 10),
				// 				b: parseInt(res[3], 10),
				// 				a: parseFloat(res[4])
				// 			};

				// 			data[i] = colorData.r;
				// 			data[i + 1] = colorData.g;
				// 			data[i + 2] = colorData.b;
				// 		}

				// 		ctx.putImageData(imageData, 0, 0);

				// 	}
				// }



			},
			async renderAnimation(itemTypes, animationName) {
				let farmeCount = this.getFrameCountByAniName(animationName);

				// this.upZr.clear();
				// this.leftZr.clear();
				// this.downZr.clear();
				// this.rightZr.clear();


				this.mainZr.playFrameCount = farmeCount;
				this.upZr.playFrameCount = farmeCount;
				this.leftZr.playFrameCount = farmeCount;
				this.downZr.playFrameCount = farmeCount;
				this.rightZr.playFrameCount = farmeCount;


				for (var i = 0; i < itemTypes.length; i++) {

					let code = itemTypes[i].split("_")[0];
					let imgInfo = this.imgCnavasList.filter(x => x.code == code)[0];

					await this.drawItem(imgInfo, itemTypes[i], animationName);

					//绘制多层组合图
					if (imgInfo.afterInfo) {
						await this.drawItem(imgInfo.afterInfo, itemTypes[i], animationName);
					}

					// await this.upZr.drawItem({
					// 	imageInfo: imgInfo,
					// 	imgType: itemTypes[i],
					// 	aniName: "up_" + animationName
					// });
				}
			},
			//核心绘制方法
			async drawItem(imgInfo, imgType, aniName) {
				await this.upZr.drawItem({
					imageInfo: imgInfo,
					imgType: imgType,
					aniName: "up_" + aniName
				});

				await this.leftZr.drawItem({
					imageInfo: imgInfo,
					imgType: imgType,
					aniName: "left_" + aniName
				});

				await this.downZr.drawItem({
					imageInfo: imgInfo,
					imgType: imgType,
					aniName: "down_" + aniName
				});

				await this.mainZr.drawItem({
					imageInfo: imgInfo,
					imgType: imgType,
					aniName: "down_" + aniName
				});

				await this.rightZr.drawItem({
					imageInfo: imgInfo,
					imgType: imgType,
					aniName: "right_" + aniName
				});
			},
			async playAnimation(animationName) {
				this.mainZr.play(this.form.fps);
				this.upZr.play(this.form.fps);
				this.leftZr.play(this.form.fps);
				this.downZr.play(this.form.fps);
				this.rightZr.play(this.form.fps);
			},
			clearAniByLayer(layerIndex) {


				for (var i = 0; i < this.mainZr.frameGroup.length; i++) {
					let frame = this.mainZr.frameGroup[i];
					let layer = frame.layers.filter(x => x.layer == layerIndex)[0];
					if (layer.zrImg) {
						layer.zrImg.attr({
							style: {
								image: " "
							}
						})
					}
				}

				for (var i = 0; i < this.upZr.frameGroup.length; i++) {
					let frame = this.upZr.frameGroup[i];
					let layer = frame.layers.filter(x => x.layer == layerIndex)[0];
					if (layer.zrImg) {
						layer.zrImg.attr({
							style: {
								image: " "
							}
						})
					}
				}


				for (var i = 0; i < this.leftZr.frameGroup.length; i++) {
					let frame = this.leftZr.frameGroup[i];
					let layer = frame.layers.filter(x => x.layer == layerIndex)[0];
					if (layer.zrImg) {
						layer.zrImg.attr({
							style: {
								image: " "
							}
						})
					}
				}


				for (var i = 0; i < this.downZr.frameGroup.length; i++) {
					let frame = this.downZr.frameGroup[i];
					let layer = frame.layers.filter(x => x.layer == layerIndex)[0];
					if (layer.zrImg) {
						layer.zrImg.attr({
							style: {
								image: " "
							}
						})
					}
				}


				for (var i = 0; i < this.rightZr.frameGroup.length; i++) {
					let frame = this.rightZr.frameGroup[i];
					let layer = frame.layers.filter(x => x.layer == layerIndex)[0];
					if (layer.zrImg) {
						layer.zrImg.attr({
							style: {
								image: " "
							}
						})
					}
				}
			},
			speedChange(val) {
				this.playAnimation();
			},
			getFrameCountByAniName(aniName) {
				let name = "down_" + aniName;
				let rets = this.anis.filter(x => x.aniName == name);

				let framePosInfo = imageHelper.getFramePos(rets[0].framePos);

				return framePosInfo.length;
			},
			async choseIconHandler(activeCode, type) {

				this.iconActive[activeCode] = type;
				await this.initSelectInfo();

				if (type == "") {
					//清除当前图层
					let layerIndex = this.jsonData.filter(x => x.code == activeCode)[0].layer;

					this.clearAniByLayer(layerIndex);

					return;
				}

				let selectItemType = [];

				this.renderAnimation(this.currentSelectType, this.aniRadio);


			},
			iconCheckHandler(checked, activeCode) {
				if (activeCode)
					this.iconActive[activeCode] = ""

				if (checked) {
					if (this.iconDefaultType[activeCode] == "") {

						this.iconActive[activeCode] = this.jsonData.filter(x => x.code == activeCode)[0].imgs[0].type
					} else {
						this.iconActive[activeCode] = this.iconDefaultType[activeCode]
					}
				}


				this.choseIconHandler(activeCode, this.iconActive[activeCode]);


			},
			aniSelectHandler(val) {

				this.renderAnimation(this.currentSelectType, val)
			},
			rightClick(item) {
				this.dialogFormVisible = true;
				this.iconEditform.id = item.id;
				this.iconEditform.code = item.code;
				this.iconEditform.type = item.type;
				this.iconEditform.desc = item.desc;
				this.iconEditform.sex = item.sex;
			},
			editIconHandler() {
				EditIconPost(this.iconEditform).then(res => {
					if (res.code == 20000) {
						Message({
							message: '成功!',
							type: 'success',
							duration: 5 * 1000
						})
					}
				})
			},
			delImgJsonHandler() {
				let _this = this;
				DelImgJsonPost({
					id: this.iconEditform.id
				}).then(res => {
					if (res.code == 20000) {
						Message({
							message: '删除成功!',
							type: 'success',
							duration: 5 * 1000
						});
						_this.dialogFormVisible = false;
						_this.init()

					}
				})
			},
			getCodeName(type) {
				let code = type.split("_")[0];
				let info = this.jsonData.filter(x => x.code == code)[0]
				if (info) {
					return info.name
				}

			},
			download() {

				let _this = this;

				_this.exportDisabled = true;
				let offscreenCanvas = document.createElement('canvas');
				offscreenCanvas.width = 832;
				offscreenCanvas.height = 1344;
				let ctx = offscreenCanvas.getContext('2d');
				ctx.imageSmoothingEnabled = false;

				let hasAfterList = [];

				for (var i = 0; i < this.imgCnavasList.length; i++) {
					hasAfterList.push(this.imgCnavasList[i]);
					if (this.imgCnavasList[i].afterInfo) {
						hasAfterList.push(this.imgCnavasList[i].afterInfo);
					}
				}

				let sortCanvas = hasAfterList.sort((a, b) => {
					return a.layer - b.layer
				})

				let codes = this.currentSelectType.map(x => x.split("_")[0]);
				console.log(codes);

				sortCanvas = sortCanvas.filter(x => {
					return codes.indexOf(x.code) > -1;
				})

				console.log("sortCanvas", sortCanvas);

				for (var i = 0; i < sortCanvas.length; i++) {
					let item = sortCanvas[i];
					if (item.canvas)
						ctx.drawImage(item.canvas, 0, 0, 832, 1344);
				}

				// 创建数据 URL
				var dataURL = offscreenCanvas.toDataURL("image/png");


				let subCanvas = document.createElement('canvas');
				subCanvas.width = 64;
				subCanvas.height = 64;
				let ctx2 = subCanvas.getContext('2d');
				ctx2.imageSmoothingEnabled = false;

				// ctx2.drawImage(offscreenCanvas, 0, 0, 832, 1344);



				let subImgData = [];

				for (var i = 0; i < this.anis.length; i++) {

					let item = this.anis[i];
					let name = item.aniName;
					let pos = item.framePos;

					let framePos = imageHelper.getFramePos(pos);

					for (var j = 0; j < framePos.length; j++) {

						ctx2.clearRect(0, 0, 64, 64);
						let subPos = framePos[j];


						let imgdx = 64 * subPos[1];
						let imgdy = 64 * subPos[0];

						ctx2.drawImage(offscreenCanvas, -imgdx, -imgdy, 832, 1344);
						subImgData.push({
							name: name,
							index: j,
							data: subCanvas.toDataURL()
						})
					}

				}

				if (this.exportType == "big") {
					//创建一个新的 a 元素
					var downloadLink = document.createElement("a");

					// 设置下载链接的属性
					downloadLink.href = dataURL;
					downloadLink.download = "catgame.png";

					// 将链接添加到文档中，模拟点击来执行下载操作，完成后再删除
					document.body.appendChild(downloadLink);
					downloadLink.click();
					document.body.removeChild(downloadLink);

					_this.exportDisabled = false;

				} else {
					ExportImgPackagePost(subImgData).then(res => {


						DownloadFile({
							zipPath: res.data
						}).then(response => {

							const url = window.URL.createObjectURL(new Blob([response.data]));
							const link = document.createElement('a');
							link.href = url;
							const fileName = 'catgame.zip'; // 文件名
							link.setAttribute('download', fileName);
							document.body.appendChild(link);
							link.click();

							_this.exportDisabled = false;
						})

					});
				}



				//----------------------------------------------




			}

		}
	}
</script>

<style lang="scss">
	.sexiang {
		.el-slider__bar {
			background: transparent
		}

		.el-slider__runway {
			background: linear-gradient(to right, red, orange, yellow, green, blue, indigo, violet);
		}
	}

	.mainPage {
		height: 80vh;
	}

	.canvasContainer {
		display: flex;
		justify-content: space-evenly;
	}

	#mainCanvas {
		border: 1px maroon solid;
		height: 320px;
		width: 320px;
	}

	.subCanvasContainer {
		display: flex;
		justify-content: space-evenly;
	}

	.subCanvas {
		height: 128px;
		width: 128px;
		border: 1px maroon solid;
	}

	.iconContainer {
		display: flex;
		flex-wrap: wrap;
	}

	.iconBox {
		border: 1px dashed #ccc;
		margin: 5px;
		//background-color: #e4edf9;
		position: relative;
	}

	.iconBoxActive {
		border: 3px solid #00aa00 !important;
	}

	.aniBox {
		height: 320px;
		width: 180px;
		display: flex;
		flex-direction: column;
		justify-content: space-around;
	}

	.colorDialogContainer {
		.el-dialog {
			position: absolute;
			left: 5%;
		}

		.el-form-item__content {
			display: flex;
			flex-direction: column;
		}
	}
</style>