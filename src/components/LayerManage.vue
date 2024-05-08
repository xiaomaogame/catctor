<template>
	<div style="height: 80vh;overflow: auto;">
		<template>
			<el-table height="600" border stripe :data="tableData" style="width: 100%">
				<el-table-column type="index" width="50" label="序号"> </el-table-column>
				<el-table-column prop="code" label="Code" width="180">
				</el-table-column>
				<el-table-column prop="name" label="姓名" width="180">
				</el-table-column>
				<el-table-column prop="layer" label="层级">
				</el-table-column>
				<el-table-column label="操作">
				      <template slot-scope="scope">
				        <el-button
				          size="mini"
				          @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
				        <el-button
				          size="mini"
				          type="danger"
				          @click="handleDelete(scope.$index, scope.row)">删除</el-button>
				      </template>
				    </el-table-column>
			</el-table>
		</template>
	</div>
</template>

<script>
	import {
		GetImgTablesPost
	} from '@/api/myApp'

	export default {
		name: 'LayerManage',
		components: {},
		data() {
			return {
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
			}
		}
	}
</script>

<style scoped>

</style>