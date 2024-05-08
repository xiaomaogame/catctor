<template>

	<div>
		<template>
			<el-form :inline="true" :model="formInline" class="demo-form-inline">

				<el-form-item label="活动区域">
					<el-select style="width: 100%;" v-model="formInline.code" placeholder="请选择类型">
							<el-option v-for="item in searchtypeListOptions" :value="item.code" :label="item.name">

							</el-option>
						</el-select>
				</el-form-item>
				<el-form-item>
					<el-button type="primary" @click="queryData">查询/刷新</el-button>
				</el-form-item>
			</el-form>
		</template>
		<div style="height: 75vh;overflow: auto;">

			<template>
				<el-table height="75vh" border stripe :data="tableData" style="width: 100%" row-key="id"
					:tree-props="{ children: 'imgs', hasChildren: 'hasChildren' }">
					<el-table-column type="index" width="50" label="序号"> </el-table-column>
					<el-table-column prop="name" label="大类" width="180">
					</el-table-column>
					<el-table-column label="图标">
						<template slot-scope="scope">
							<el-image v-if="scope.row.iconData" style="width: 64px; height: 64px"
								:src="scope.row.iconData"></el-image>
						</template>
					</el-table-column>

					<el-table-column prop="type" label="名称（唯一）" width="180">
					</el-table-column>
					<el-table-column prop="sex" label="性别">
					</el-table-column>
					<el-table-column prop="pos" label="前后关系">
					</el-table-column>
					<el-table-column prop="id" label="素材Id">
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
					<el-form-item label="前后双图层">
						<el-radio v-model="iconEditform.pos" label="前">前</el-radio>
						<el-radio v-model="iconEditform.pos" label="后">后</el-radio>
						<el-radio v-model="iconEditform.pos" label="">无</el-radio>
					</el-form-item>
					<el-form-item label="后置Id" v-if="iconEditform.pos=='前'">
						<el-input v-model="iconEditform.afterId" autocomplete="off"></el-input>
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
	</div>

</template>

<script>
import {
	EditIconPost,
	GetImgDataPost,
	GetImgTablesPost,
	DelImgJsonPost
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
				sex: "",
				pos:"",
				afterId:""
			},
			formInline: {
				code: ''
			},
			dialogFormVisible: false,
			tableData: [],
			typeListOptions: [], //所有项目的类型列表
			searchtypeListOptions: [], //所有项目的类型列表
		}
	},
	mounted() {
		let _this = this;
		GetImgTablesPost({}).then(res => {
			_this.typeListOptions = res.data;
			
			_this.searchtypeListOptions = [...[{code:"",name:"全部"}],...res.data];

		});

		this.queryData()
	},
	methods: {
		queryData() {
			let _this = this;
			GetImgDataPost({code:_this.formInline.code}).then(res => {
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
			this.iconEditform.afterId = row.afterId;
			this.iconEditform.pos = row.pos==null?"":row.pos;
		},
		handleDelete(index, row) {

			let _this = this;

			MessageBox.confirm('此操作将删除该元素, 是否继续?', '提示', {
				confirmButtonText: '确定',
				cancelButtonText: '取消',
				type: 'warning'
			}).then(() => {

				DelImgJsonPost({
					id: row.id
				}).then(res => {
					if (res.code == 20000) {
						Message({
							message: '删除成功!',
							type: 'success',
							duration: 5 * 1000
						});
						_this.dialogFormVisible = false;
						_this.queryData()

					}
				}).catch(() => {

				})
			}).catch(() => {

			});


		},
		editIconHandler() {
			let _this = this;
			EditIconPost(this.iconEditform).then(res => {
				if (res.code == 20000) {
					Message({
						message: '成功!',
						type: 'success',
						duration: 5 * 1000
					});
					_this.queryData();
					_this.dialogFormVisible = false;
				}
			}).catch(() => {

			})
		}
	}
}
</script>

<style scoped></style>