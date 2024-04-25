<template>
	<div>
		<el-row>
			<el-col :span="6">
				<el-card class="box-card">
					<div>
						<el-collapse>
							<el-collapse-item v-for="iconItem in iconList">
								<template slot="title" @click.stop>
									<el-checkbox :value="iconActive[iconItem.code]!=''?true:false"
										@click.stop.native="()=>{}"
										@change="(checked)=>iconCheckHandler(checked,iconItem.code)">{{iconItem.name}}</el-checkbox>
								</template>
								<div class="iconContainer">
									<div class="iconBox"
										:class="iconActive[iconItem.code]==icon.type?'iconBoxActive':''"
										v-for="icon in iconItem.imgs">
										<el-image style="width: 64px; height: 64px"
											:src="'http://localhost:5120/'+icon.iconUrl" fit="fill"
											@click="choseIconHandler(iconItem.code,icon.type)"></el-image>
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
		<el-row>

		</el-row>
	</div>

</template>

<script>
	import zrTool from "@/utils/ZrenderTool"
	import {
		jsonData,
		anis
	} from '@/imageData/imgData.js'

	import * as zrender from 'zrender'

	export default {
		name: "Home",
		data() {
			return {
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
				aniRadio: "walk",
				iconDefaultType: {
					body: "body_male",
					head: "head_human_male"
				},
				iconActive: {
					body: "",
					head: ""
				},
				iconList: [],
				currentSelectType: ["body_male"]
			}
		},
		mounted() {
			this.initIcon();
			
			console.log(this.currentSelectType);
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
				this.iconList = jsonData.data;
				let selectItemType = [];
				for (let key in this.iconDefaultType) {
					selectItemType.push(this.iconDefaultType[key])
				}
				this.currentSelectType = selectItemType;
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
			speedChange(val) {
				this.playAnimation();
			},
			getFrameCountByAniName(aniName) {
				let name = "down_" + aniName;
				let rets = anis.filter(x => x.aniName == name);

				let framePosInfo = this.mainZr.getFramePos(rets[0].framePos);

				return framePosInfo.length;
			},
			choseIconHandler(activeCode, type) {

				// for (var i = 0; i < Things.length; i++) {
				// 	Things[i]
				// }
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

				if (checked)
					this.iconActive[activeCode] = this.iconDefaultType[activeCode]
			},
			aniSelectHandler(val) {
				this.renderAnimation(this.currentSelectType, val)
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