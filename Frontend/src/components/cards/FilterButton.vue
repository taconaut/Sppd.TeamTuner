<template>
  <img
    :class="['bt-filter', 'border border-dark', 'rounded', 'p-1', isActive ? 'bt-filter-enabled' : 'bt-filter-disabled']"
    height="36px"
    :src="imgSrc"
    @click="emitToggleIsActive()"
  />
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  name: 'FilterButtonComponent',
  props: {
    isActive: Boolean,
    imgSrc: String
  },
  methods: {
    emitToggleIsActive() {
      this.$emit('isActive', !this.isActive)
    }
  }
})
</script>

<style scoped>
.bt-filter {
  cursor: pointer;
}
.bt-filter-enabled {
  background: rgb(33, 37, 41);
}
.bt-filter-disabled {
  background: transparent;
}
</style>
