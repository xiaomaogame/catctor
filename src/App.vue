<template>
  <div id="app" class="page">
	<Home></Home>
  </div>
</template>

<script>
import Home from './components/Home.vue'

export default {
  name: 'App',
  components: {
    Home
  }
}
</script>

<style lang="scss">
	.page{
		margin-top: 10px;
		margin-left: 10px;
		margin-bottom: 10px;
	}
</style>
