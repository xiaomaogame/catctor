import Vue from 'vue'
import App from './App.vue'

Vue.config.productionTip = false

import ElementUI from 'element-ui';
//样式文件需要单独引入
import 'element-ui/lib/theme-chalk/index.css';

Vue.use(ElementUI);


new Vue({
  render: h => h(App),
}).$mount('#app')
