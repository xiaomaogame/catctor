<template>
	<div>
		<el-upload class="upload-demo" drag action="https://jsonplaceholder.typicode.com/posts/" multiple :limit="1"
			:before-upload="beforeAvatarUpload" :on-success="handleAvatarSuccess">
			<i class="el-icon-upload"></i>
			<div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
			<div class="el-upload__tip" slot="tip">只能上传jpg/png文件，且不超过500kb</div>


		</el-upload>
		<img v-if="imageUrl" :src="imageUrl" class="avatar">
		
		<div id="uploadCanvas" style="height: 100px;width: 100px;"></div>
	</div>
</template>

<script>
	
	import * as zrender from 'zrender'
	
	export default {
		name: 'UploadFile',
		components: {},
		data() {
			return {
				zr:null,
				imageUrl: ""
			}
		},
		mounted() {
			this.zr = zrender.init(document.getElementById("uploadCanvas"));
		},
		methods: {
			beforeAvatarUpload(file) {
				const isJPG = file.type === 'image/png';
				const isLt2M = file.size / 1024 / 1024 < 2;

				if (!isJPG) {
					this.$message.error('上传头像图片只能是 JPG 格式!');
					return;
				}
				if (!isLt2M) {
					this.$message.error('上传头像图片大小不能超过 2MB!');
					return;
				}
				return isJPG && isLt2M;
			},
			handleAvatarSuccess(res, file) {
				//this.imageUrl = URL.createObjectURL(file.raw);
				let img = new zrender.Image({
					style:{
						image:URL.createObjectURL(file.raw),
						x:0,
						y:0,
						width:64,
						height:64
					}
				});
				
				this.zr.add(img);
			},
		}
	}
</script>

<style scoped>
	.avatar {
		width: 178px;
		height: 178px;
	}
</style>