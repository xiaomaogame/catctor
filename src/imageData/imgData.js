const anis = [{
	"aniName": "up_fashu",
	"framePos": [
		[0, 0, 6]
	]
}, {
	"aniName": "left_fashu",
	"framePos": [
		[1, 0, 6]
	]
}, {
	"aniName": "down_fashu",
	"framePos": [
		[2, 0, 6]
	]
}, {
	"aniName": "right_fashu",
	"framePos": [
		[3, 0, 6]
	]
}, {
	"aniName": "up_tuci",
	"framePos": [
		[4, 0, 7]
	]
}, {
	"aniName": "left_tuci",
	"framePos": [
		[5, 0, 7]
	]
}, {
	"aniName": "down_tuci",
	"framePos": [
		[6, 0, 7]
	]
}, {
	"aniName": "right_tuci",
	"framePos": [
		[7, 0, 7]
	]
}, {
	"aniName": "up_walk",
	"framePos": [
		[8, 1, 8]
	]
}, {
	"aniName": "left_walk",
	"framePos": [
		[9, 1, 8]
	]
}, {
	"aniName": "down_walk",
	"framePos": [
		[10, 1, 8]
	]
}, {
	"aniName": "right_walk",
	"framePos": [
		[11, 1, 8]
	]
}, {
	"aniName": "up_fangyu",
	"framePos": [
		[12, 0, 5]
	]
}, {
	"aniName": "left_fangyu",
	"framePos": [
		[13, 0, 5]
	]
}, {
	"aniName": "down_fangyu",
	"framePos": [
		[14, 0, 5]
	]
}, {
	"aniName": "right_fangyu",
	"framePos": [
		[15, 0, 5]
	]
}, {
	"aniName": "up_shejian",
	"framePos": [
		[16, 0, 5]
	]
}, {
	"aniName": "left_shejian",
	"framePos": [
		[17, 0, 5]
	]
}, {
	"aniName": "down_shejian",
	"framePos": [
		[18, 0, 5]
	]
}, {
	"aniName": "right_shejian",
	"framePos": [
		[19, 0, 5]
	]
}, {
	"aniName": "down_die",
	"framePos": [
		[20, 0, 5]
	]
}];

const jsonData = {
	version: "1.0",
	data: [{
		name: "头部",
		code: "head",
		iconPos: [7, 0],
		imgs: [{
			"imgUrl": "head/head_human_male.png",
			"iconUrl": "head/head_human_male_icon.png",
			"type": "head_human_male",
			"layer": 1
		}, {
			"imgUrl": "head/head_human_female.png",
			"iconUrl": "head/head_human_female_icon.png",
			"type": "head_human_female",
			"layer": 1
		}, {
			"imgUrl": "head/head_boarman.png",
			"iconUrl": "head/head_boarman_icon.png",
			"type": "head_boarman",
			"layer": 1
		}]
	}, {
		name: "身体",
		code: "body",
		iconPos: [7, 0],
		imgs: [{
				"imgUrl": "body/body_male.png",
				"iconUrl": "body/body_male_icon.png",
				"type": "body_male",
				"layer": 0
			},
			{
				"imgUrl": "body/body_female.png",
				"type": "body_female",
				"iconUrl": "body/body_female_icon.png",
				"layer": 0
			},
			{
				"imgUrl": "body/body_muscular.png",
				"type": "body_muscular",
				"iconUrl": "body/body_muscular_icon.png",
				"layer": 0
			},
			{
				"imgUrl": "body/body_pregnant.png",
				"type": "body_pregnant",
				"iconUrl": "body/body_pregnant_icon.png",
				"layer": 0
			},
			{
				"imgUrl": "body/body_skeleton.png",
				"type": "body_skeleton",
				"iconUrl": "body/body_skeleton_icon.png",
				"layer": 0
			},
		]
	}]
}

export {
	jsonData,
	anis
};