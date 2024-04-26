import axios from 'axios'
import {
	MessageBox,
	Message
} from 'element-ui'

// 创建一个 axios 实例
const service = axios.create({
	baseURL: "http://localhost:21422", // url = base url + request url
	// withCredentials: true, // 发起跨域请求时发送 cookie
	timeout: 0, // 请求超时时间,
	needErrDialog: true
})

// 请求拦截器
service.interceptors.request.use(
	config => {
		return config
	},
	error => {
		return Promise.reject(error)
	}
)

// 响应拦截器
service.interceptors.response.use(
	response => {
		const res = response.data
		if (res.code == 40000) {

			Message({
				message: res.message,
				type: 'error',
				duration: 5 * 1000
			})

			return Promise.reject(new Error(res.message || '错误'))
		} else {
			
			return res
		}
	},
	error => {
		Message({
			message: "错误",
			type: 'error',
			duration: 5 * 1000
		})
		return Promise.reject(error)
	}
)

export default service