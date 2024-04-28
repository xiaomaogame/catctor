<template>
	<div>
		<el-row>
			<el-col :span="6">
				<el-card class="box-card">
					<div style="height: 80vh;overflow: auto;">
						<el-collapse>
							<el-collapse-item v-for="iconItem in iconList">
								<template slot="title" @click.stop>
									<el-checkbox :value="iconActive[iconItem.code] != '' ? true : false"
										@click.stop.native="() => { }"
										@change="(checked) => iconCheckHandler(checked, iconItem.code)">{{ iconItem.name+'  ('+iconItem.imgs.length+')' }}</el-checkbox>
								</template>
								<div class="iconContainer">
									<div class="iconBox"
										:class="iconActive[iconItem.code] == icon.type ? 'iconBoxActive' : ''"
										v-for="icon in iconItem.imgs">
										<el-image style="width: 64px; height: 64px" :src="icon.iconData" fit="fill"
											@click="choseIconHandler(iconItem.code, icon.type)"
											@contextmenu.prevent="rightClick(icon)"></el-image>
											<i v-if="icon.sex=='男'" class="el-icon-male" style="color: blue;position: absolute;right: 0;bottom: 0;"></i>
										    <i v-if="icon.sex=='女'" class="el-icon-female" style="color: hotpink;position: absolute;right: 0;bottom: 0;"></i>

									</div>


								</div>
							</el-collapse-item>
						</el-collapse>
					</div>
				</el-card>


				<el-color-picker v-model="color" color-format="hsl" @change="colorChange"></el-color-picker>
				<button @click="colorchangetest">色相bianh测试</button>
			</el-col>
			<el-col :span="12">
				<el-card class="box-card">
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
							<el-form ref="form" :model="form" label-width="50px">
								<el-form-item label="速度">
									<el-slider v-model="form.fps" :min="1" :max="30" @change="speedChange"></el-slider>
								</el-form-item>
							</el-form>
						</div>
					</div>

				</el-card>
			</el-col>
			<el-col :span="6">

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
	</div>

</template>

<script>
	import zrTool from "@/utils/ZrenderTool"
	import CharacterData from "@/imageData/characterData"
	import * as zrender from 'zrender'

	import {
		GetImgTablesPost,
		EditIconPost,
		DelImgJsonPost
	} from '@/api/myApp'

	import {
		MessageBox,
		Message
	} from 'element-ui'

	export default {
		name: "Home",
		data() {
			return {
				dialogFormVisible: false,
				anis: [],
				mainZr: null,
				upZr: null,
				leftZr: null,
				downZr: null,
				rightZr: null,
				loading: false,
				timer: null,
				farmeCount: 0,
				color: null,
				form: {
					name: "",
					fps: 12,
					scale: 100
				},
				iconEditform: {
					id: "",
					code: "",
					desc: "",
					type: "",
					sex: ""
				},
				typeListOptions: [],
				aniRadio: "walk",
				iconDefaultType: {
					body: "body_male",
					head: "head_male",
					hair: "",
					belt: "",
					facial: "",
					torso: "",
					shoulders: "",
					hands: "",
					feet: "",
					legs: "",
					weapons: "",
					shadow: "",
					arms: "",
					backpack: "",
					hat: ""
				},
				iconActive: {
					body: "",
					head: "",
					hair: "",
					belt: "",
					facial: "",
					torso: "",
					shoulders: "",
					hands: "",
					feet: "",
					legs: "",
					weapons: "",
					shadow: "",
					arms: "",
					backpack: "",
					hat: ""
				},
				iconList: [],
				currentSelectType: ["body_male"]
			}
		},
		async mounted() {

			let characterData = new CharacterData();
			await characterData.init();
			this.anis = characterData.anis;
			this.iconList = characterData.jsonData;

			this.initIcon();
			await this.initTypeList();
			this.initZr();

			this.renderAnimation(this.currentSelectType, this.aniRadio)
			this.playAnimation();
		},
		methods: {
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
			initIcon() {
				let selectItemType = [];
				for (let key in this.iconDefaultType) {
					if (this.iconDefaultType[key] != "")
						selectItemType.push(this.iconDefaultType[key])
				}

				this.currentSelectType = selectItemType;
			},
			async initTypeList() {
				let res = await GetImgTablesPost({});
				this.typeListOptions = res.data;
			},
			colorChange(value) {


				let regex = /hsl\((\d+),\s*(\d+)%,\s*(\d+)%\)/;
				let result = value.match(regex);
				console.log(result)
				if (result) {

					let hsl = {
						h: result[1],
						s: result[2] / 100,
						l: result[3] / 100
					}


					this.mainZr.changeColor({
						imgType: "body_male",
						aniName: "down_walk",
						hsl: hsl
					});

				} else {
					console.log('没有找到匹配的HSL值');
				}


				console.log(value)
			},
			colorchangetest() {
				this.upZr.changeColor({
					imgType: "body_male",
					aniName: "up_walk"
				});
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
					await this.upZr.drawItem({
						imgType: itemTypes[i],
						aniName: "up_" + animationName
					});

					await this.leftZr.drawItem({
						imgType: itemTypes[i],
						aniName: "left_" + animationName
					});

					await this.downZr.drawItem({
						imgType: itemTypes[i],
						aniName: "down_" + animationName
					});

					await this.mainZr.drawItem({
						imgType: itemTypes[i],
						aniName: "down_" + animationName
					});

					await this.rightZr.drawItem({
						imgType: itemTypes[i],
						aniName: "right_" + animationName
					});
				}
			},
			async playAnimation(animationName) {
				this.mainZr.play(this.form.fps);
				this.upZr.play(this.form.fps);
				this.leftZr.play(this.form.fps);
				this.downZr.play(this.form.fps);
				this.rightZr.play(this.form.fps);
			},
			clearAniByLayer(layerIndex) {
				console.log("imggroup", this.mainZr.frameGroup);

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

				let framePosInfo = this.mainZr.getFramePos(rets[0].framePos);

				return framePosInfo.length;
			},
			choseIconHandler(activeCode, type) {

				if (type == "") {
					//清除当前图层
					let layerIndex = this.iconList.filter(x => x.code == activeCode)[0].layer;

					this.clearAniByLayer(layerIndex);
					return;
				}


				this.iconActive[activeCode] = type;

				let selectItemType = [];

				for (let key in this.iconActive) {
					selectItemType.push(this.iconActive[key])
				}
				this.currentSelectType = selectItemType;

				this.renderAnimation(this.currentSelectType, this.aniRadio);


			},
			iconCheckHandler(checked, activeCode) {
				if (activeCode)
					this.iconActive[activeCode] = ""

				if (checked) {
					if (this.iconDefaultType[activeCode] == "") {
						console.log("222", this.iconList.filter(x => x.code == activeCode))
						this.iconActive[activeCode] = this.iconList.filter(x => x.code == activeCode)[0].imgs[0].type
					} else {
						this.iconActive[activeCode] = this.iconDefaultType[activeCode]
					}
				}

				this.initIcon()
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
				DelImgJsonPost({
					id: this.iconEditform.id
				}).then(res => {
					if (res.code == 20000) {
						Message({
							message: '删除成功!',
							type: 'success',
							duration: 5 * 1000
						})
					}
				})
			}

		}
	}
</script>

<style lang="scss">
	.mainPage {
		height: 85vh;
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
		margin-left: 5px;
		margin-top: 5px;
		background-color: #e4edf9;
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
</style>