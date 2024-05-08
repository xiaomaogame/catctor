// singleton.js
let instance;


import {
	GetAniInfoPost,
	GetImgDataPost
} from '@/api/myApp'
import {
	init
} from 'zrender';



export default class CharacterData {
	constructor() {
		if (!instance) {
			this.anis = null;
			this.jsonData = null;
			instance = this;
		}

		return instance;
	}

	async init() {
		
		if (!this.anis) {
			let res = await GetAniInfoPost();
			this.anis = res.data;
		}

		if (!this.jsonData) {
			let res = await GetImgDataPost({code:""});
			this.jsonData = res.data;
		}
		

	}
}