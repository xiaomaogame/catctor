var jsonData = null;
var anis = null;

import {
	GetAniInfoPost,
	GetImgDataPost
} from '@/api/myApp'

export async function GetAniInfo() {
   if(anis==null)
   {
		let res = await GetAniInfoPost({});
		anis = res.data;
   }
   return anis;
}

export async function GetImgData() {
	if(jsonData==null)
	{
		 let res = await GetImgDataPost({code:""});
		 jsonData = res.data;
	}

	return jsonData;
 }