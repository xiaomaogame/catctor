import Vue from 'vue'
import VueRouter from 'vue-router'
import Router from 'vue-router'
Vue.use(Router)

import Home from '@/components/Home.vue'
import UploadFile from '@/components/UploadFile.vue'

const router = new VueRouter({
	routes: [{
		path: "/",
		component: Home
	}, {
		path: "/UploadFile",
		component: UploadFile
	}]
})

export default router;
