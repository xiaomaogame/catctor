<template>
	<div>
		<el-row>
			<el-col :span="6">
				<button @click="test2">测试</button>
			</el-col>
			<el-col :span="12">
				<el-card class="box-card">
					<div class="mainPage">
						<div class="canvasContainer" style="margin-bottom: 20px;">
							<div id="mainCanvas"></div>
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
import { jsonData, anis } from '@/imageData/imgData.js'

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
			form: {
				name: "",
				fps: 1,
				scale: 100
			}
		}
	},
	mounted() {
		this.upZr = new zrTool("upCanvas", 64);
		this.leftZr = new zrTool("leftCanvas", 64);
		this.downZr = new zrTool("downCanvas", 64);
		this.rightZr = new zrTool("rightCanvas", 64);

	},
	methods: {
		test2() {
			this.playAnimation("walk");
		},
		async playAnimation(animationName) {

			let farmeCount = this.getFrameCountByAniName(animationName);

			this.upZr.clear();
			this.leftZr.clear();
			this.downZr.clear();
			this.rightZr.clear();


			this.upZr.playFrameCount = farmeCount;
			this.leftZr.playFrameCount = farmeCount;
			this.downZr.playFrameCount = farmeCount;
			this.rightZr.playFrameCount = farmeCount;


			await this.upZr.drawItem("body_male", "up_" + animationName);
			await this.upZr.drawItem("hair_male", "up_" + animationName);

			await this.leftZr.drawItem("body_male", "left_" + animationName);
			await this.leftZr.drawItem("hair_male", "left_" + animationName);

			await this.downZr.drawItem("body_male", "down_" + animationName);
			await this.downZr.drawItem("hair_male", "down_" + animationName);

			await this.rightZr.drawItem("body_male", "right_" + animationName);
			await this.rightZr.drawItem("hair_male", "right_" + animationName);

			this.upZr.play(this.form.fps);
			this.leftZr.play(this.form.fps);
			this.downZr.play(this.form.fps);
			this.rightZr.play(this.form.fps);


		},
		speedChange(val) {

			this.upZr.play(this.form.fps);
			this.leftZr.play(this.form.fps);
			this.downZr.play(this.form.fps);
			this.rightZr.play(this.form.fps);
		},
		getFrameCountByAniName(aniName) {
			let name = "down_" + aniName;
			let rets = anis.filter(x => x.aniName == name);

			console.log(rets);

			return rets[0].frameCount;
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
	justify-content: center;
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
</style>