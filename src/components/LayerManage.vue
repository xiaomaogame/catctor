<template>
	<div>
		<template>
			<el-form :inline="true" class="demo-form-inline">


				<el-form-item>
					<el-button @click="AddLayerShow">
						新增图层
					</el-button>
				</el-form-item>
			</el-form>
		</template>
		<div style="height: 75vh;overflow: auto;">
			<template>
				<el-table height="75vh" border stripe :data="tableData" style="width: 100%">
					<el-table-column type="index" width="50" label="序号"> </el-table-column>
					<el-table-column prop="code" label="Code" width="180">
					</el-table-column>
					<el-table-column prop="name" label="姓名" width="180">
					</el-table-column>
					<el-table-column prop="layer" label="层级">
					</el-table-column>
					<el-table-column label="操作">
						<template slot-scope="scope">
							<el-button size="mini" @click="handleEditShow(scope.$index, scope.row)">编辑</el-button>
							<el-button size="mini" type="danger"
								@click="handleDelete(scope.$index, scope.row)">删除</el-button>
						</template>
					</el-table-column>
				</el-table>
			</template>

			<el-dialog :title="title" :visible.sync="dialogFormVisible" width="30%">
				<el-form :model="layerEditform" label-width="100px">
					<el-form-item label="Code">
						<el-input v-model="layerEditform.code" autocomplete="off"></el-input>
					</el-form-item>

					<el-form-item label="名称">
						<el-input v-model="layerEditform.name" autocomplete="off"></el-input>
					</el-form-item>
					<el-form-item label="层级">
						<el-input-number v-model="layerEditform.layer" :min="-30" :max="30"
							label="层级"></el-input-number>

					</el-form-item>
				</el-form>
				<div slot="footer" class="dialog-footer">
					<el-button @click="dialogFormVisible = false">取 消</el-button>
					<el-button type="primary" @click="editLayerHandler">确 定</el-button>
				</div>
			</el-dialog>

		</div>
	</div>

</template>

<script>
import {
	DelImgLayer,
	UpdateImgLayer,
	GetImgTablesPost,
	AddImgLayer
} from '@/api/myApp'

import {
	MessageBox,
	Message
} from 'element-ui'

export default {
	name: 'LayerManage',
	components: {},
	data() {
		return {
			title: "编辑",
			layerEditform: {
				code: "",
				name: "",
				id: "",
				layer: ""
			},
			dialogFormVisible: false,
			tableData: [{
				date: '2016-05-02',
				name: '王小虎',
				address: '上海市普陀区金沙江路 1518 弄'
			}, {
				date: '2016-05-04',
				name: '王小虎',
				address: '上海市普陀区金沙江路 1517 弄'
			}, {
				date: '2016-05-01',
				name: '王小虎',
				address: '上海市普陀区金沙江路 1519 弄'
			}, {
				date: '2016-05-03',
				name: '王小虎',
				address: '上海市普陀区金沙江路 1516 弄'
			}]
		}
	},
	mounted() {
		this.queryData()
	},
	methods: {
		queryData() {
			let _this = this;
			GetImgTablesPost({}).then(res => {
				if (res.code == 20000) {
					_this.tableData = res.data;
				}
			})
		},
		handleEditShow(index, row) {
			console.log("row", row)
			this.dialogFormVisible = true;
			this.layerEditform.id = row.id;
			this.layerEditform.code = row.code;
			this.layerEditform.name = row.name;
			this.layerEditform.layer = row.layer;


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