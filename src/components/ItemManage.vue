<template>
	<div style="height: 80vh;overflow: auto;">
		<template>
			<el-table height="600" border stripe :data="tableData" style="width: 100%" row-key="id"
				:tree-props="{ children: 'imgs', hasChildren: 'hasChildren' }">
				<el-table-column type="index" width="50" label="序号"> </el-table-column>
				<el-table-column prop="name" label="大类" width="180">
				</el-table-column>
				<el-table-column label="图标">
					<template slot-scope="scope">
						<el-image v-if="scope.row.iconData" style="width: 64px; height: 64px" :src="scope.row.iconData"
							:fit="fit"></el-image>
					</template>
				</el-table-column>

				<el-table-column prop="type" label="名称（唯一）" width="180">
				</el-table-column>
				<el-table-column prop="sex" label="性别">
				</el-table-column>
				<el-table-column prop="pos" label="前后关系">
				</el-table-column>
				<el-table-column prop="desc" label="备注">
				</el-table-column>
				<el-table-column label="操作">
					<template slot-scope="scope" v-if="scope.row.type">
						<el-button size="mini" @click="handleEditShow(scope.$index, scope.row)">编辑</el-button>
						<el-button size="mini" type="danger"
							@click="handleDelete(scope.$index, scope.row)">删除</el-button>
					</template>
				</el-table-column>
			</el-table>
		</template>

		<el-dialog :title="title" :visible.sync="dialogFormVisible" width="30%">
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

				<el-button @click="dialogFormVisible = false">取 消</el-button>
				<el-button type="primary" @click="editIconHandler">确 定</el-button>
			</div>
		</el-dialog>

	</div>
</template>

<script>
import {
	GetImgDataPost
} from '@/api/myApp'

import {
	MessageBox,
	Message
} from 'element-ui'

export default {
	name: 'ItemManage',
	components: {},
	data() {
		return {
			title: "编辑",
			iconEditform: { //图标编辑信息
				id: "",
				code: "",
				desc: "",
				type: "",
				sex: ""
			},
			dialogFormVisible: false,
			tableData: []
		}
	},
	mounted() {
		this.queryData()
	},
	methods: {
		queryData() {
			let _this = this;
			GetImgDataPost({}).then(res => {
				if (res.code == 20000) {
					for (let index = 0; index < res.data.length; index++) {
						const element = res.data[index];
						element.id = (index + 1) * 10000
					}
					_this.tableData = res.data;
					console.log(_this.tableData)
				}
			})
		},
		handleEditShow(index, row) {
			console.log("row", row)
			this.dialogFormVisible = true;
			this.iconEditform.id = row.id;
			this.iconEditform.code = row.code;
			this.iconEditform.type = row.type;
			this.iconEditform.desc = row.desc;
			this.iconEditform.sex = row.sex;
		},
		editLayerHandler() {


			let _this = this;

			if (_this.title == "新增") {
				AddImgLayer(this.layerEditform).then(res => {
					if (res.code == 20000) {
						_this.queryData();
						_this.dialogFormVisible = false;
						Message({
							message: '成功!',
							type: 'success',
							duration: 5 * 1000
						})
					}
				}).catch(() => {

				})
			} else {
				UpdateImgLayer(this.layerEditform).then(res => {
					if (res.code == 20000) {
						_this.queryData();
						_this.dialogFormVisible = false;
						Message({
							message: '成功!',
							type: 'success',
							duration: 5 * 1000
						})
					}
				}).catch(() => {

				})
			}
		},
		handleDelete(index, row) {

			let _this = this;

			MessageBox.confirm('此操作将删除该层级, 是否继续?', '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning'
			}).then(() => {
				DelImgLayer(row).then(res => {
					if (res.code == 20000) {
						_this.queryData();
						_this.dialogFormVisible = false;
						Message({
							message: '成功!',
							type: 'success',
							duration: 5 * 1000
						})
					}
				}).catch(() => {

				})
			}).catch(() => {

			});


		},
		AddLayerShow() {
			this.title = "新增";
			this.dialogFormVisible = true;
			this.layerEditform.id = "";
			this.layerEditform.code = "";
			this.layerEditform.name = "";
			this.layerEditform.layer = "";
		}
	}
}
</script>

<style scoped></style>