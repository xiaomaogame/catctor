const { defineConfig } = require('@vue/cli-service')

console.log(process.env.NODE_ENV,'环境')
module.exports = defineConfig({
  transpileDependencies: true,
  lintOnSave: false 
})
