<template>
	<div>
		<el-row>
			<el-col :span="8">
				<button @click="test2">测试</button>
			</el-col>
			<el-col :span="8">
				<el-card class="box-card">
					<div class="mainPage">
						<div class="canvasContainer" style="margin-bottom: 20px;">
							<div id="mainCanvas"></div>
						</div>
						<div>
							<el-form ref="form" :model="form" label-width="50px">
								<el-form-item label="速度">
									<el-slider v-model="form.speed" :step="10"></el-slider>
								</el-form-item>
								<el-form-item label="缩放">
									<el-slider v-model="form.scale" :step="10"></el-slider>
								</el-form-item>
							</el-form>
						</div>
					</div>

				</el-card>
			</el-col>
			<el-col :span="8">

			</el-col>

		</el-row>
		<el-row>

		</el-row>
	</div>

</template>

<script>
import zrTool from "@/utils/ZrenderTool"

export default {
	name: "Home",
	data() {
		return {
			loading: false,
			timer:null,
			farmeCount:0,
			form: {
				name: "",
				speed:50,
				scale:100
			}
		}
	},
	mounted() {
		let _this = this;

		zrTool.initZr("mainCanvas");

	},
	methods: {
		async test2() {

			await this.drawItem("body_male", "left_walk");
			await this.drawItem("hair_male", "left_walk");
			this.frameCount = 9;
			this.play();
			//this.hideAll()

		},
		async drawItem(type, ani) {
			let _this = this;
			let frameInfo = zrTool.findFrameInfo(type, ani);

			console.log(frameInfo);
			let sourceImage = await zrTool.loadImage(require("@/assets/" + frameInfo.imgUrl));

			for (var i = 0; i < frameInfo.frameCount; i++) {
				let img = zrTool.getZrImage(sourceImage, zrTool.canvasSize * i, frameInfo.rowIndex * zrTool.canvasSize);
				img.hide()
				zrTool.fillFrameImg(i, frameInfo.type, img, frameInfo.layer);
				zrTool.drawFrame(i);
			}
		},
		hideAll() {
			for (var i = 0; i < zrTool.frameGroup.length; i++) {
				//console.log( zrTool.frameGroup[i])
				for (var j = 0; j < zrTool.frameGroup[i].layers.length; j++) {
					if (zrTool.frameGroup[i].layers[j].zrImg != null) {
						zrTool.frameGroup[i].layers[j].zrImg.hide();
					}
				}
			}

		},
		play() {
			let index = 0;
			let _this = this;

			if(_this.timer!=null)
			{
					clearInterval(_this.timer);
			}

			_this.timer = setInterval(() => {

				_this.hideAll()

				for (var i = 0; i < zrTool.frameGroup[index].layers.length; i++) {
					if (zrTool.frameGroup[index].layers[i].zrImg != null)
						zrTool.frameGroup[index].layers[i].zrImg.show();
				}

				index++;

				if (index >= _this.frameCount) {
					index = 0;
				}
			}, 45)

		},
		async test() {
			await zrTool.drawImageGroup();

			for (var i = 0; i < zrTool.imageGroup.length; i++) {
				let item = zrTool.imageGroup[i];
				item.hide();
				zrTool.zr.add(item)
			}

			let index = 0;

			setInterval(() => {
				for (var i = 0; i < zrTool.imageGroup.length; i++) {
					let item = zrTool.imageGroup[i];
					item.hide();
				}
				zrTool.imageGroup[index].show();
				index++;
				if (index >= zrTool.imageGroup.length) {
					index = 0;
				}
			}, 100)
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
</style>