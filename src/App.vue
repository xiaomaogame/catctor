<template>
	<div id="app" class="page">
		<el-menu :default-active="activeIndex" class="el-menu-demo" mode="horizontal" @select="handleSelect">
			<el-menu-item index="/">主页</el-menu-item>
			<el-menu-item index="/UploadFile">上传文件</el-menu-item>
		</el-menu>
		<div style="padding-top: 10px;">
			<keep-alive>
				<router-view></router-view>
			</keep-alive>
		</div>

	</div>
</template>

<script>
	import Home from './components/Home.vue'

	export default {
		name: 'App',
		components: {
			Home
		},
		data() {
			return {
				activeIndex: '/'
			}
		},
		mounted() {
			this.activeIndex  = this.$route.path;
		},
		watch: {
			'$route'(to, from) {
				// 当路由变化时，你想要做的事情
				//console.log(to)
			}
		},
		methods: {
			handleSelect(key, keyPath) {
				this.$router.push(key).catch(err => {});
			}
		}
	}
</script>

<style lang="scss">
	.page {
		margin-top: 10px;
		margin-left: 10px;
		margin-bottom: 10px;
	}
</style>