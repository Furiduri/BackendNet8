<template>
  <div class="home-page p-20px">
    <el-card class="max-w-800px m-auto">
      <template #header>
        <div class="flex items-center">
          <el-icon class="mr-2 text-2xl text-primary"><House /></el-icon>
          <span class="text-xl font-bold">Bienvenido, {{ userAccountStore.userInfo?.username || userAccountStore.userInfo?.userName }}</span>
        </div>
      </template>

      <div class="home-content py-4">
        <p class="text-lg mb-4">
          Esta es tu página de inicio personalizada. Aquí puedes ver un resumen de tu cuenta.
        </p>

        <el-descriptions title="Información de Usuario" :column="1" border>
          <el-descriptions-item label="Nombre de Usuario">
            {{ userAccountStore.userInfo?.username || userAccountStore.userInfo?.userName }}
          </el-descriptions-item>
          <el-descriptions-item label="Email">
            {{ userAccountStore.userInfo?.email || userAccountStore.userInfo?.Email }}
          </el-descriptions-item>
          <el-descriptions-item label="Roles">
            <div class="flex flex-wrap gap-2">
              <el-tag
                v-for="role in (userAccountStore.userInfo?.roles || userAccountStore.userInfo?.Roles)"
                :key="role"
                type="success"
                effect="dark"
              >
                {{ role }}
              </el-tag>
            </div>
          </el-descriptions-item>
        </el-descriptions>

        <div class="mt-8 flex justify-center gap-4">
          <el-button type="primary" @click="goToProjects">
            Ir a Proyectos
          </el-button>
          <el-button v-if="userAccountStore.isAdminOrDev" type="warning" @click="goToUserManagement">
            Gestión de Usuarios
          </el-button>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { House } from '@element-plus/icons-vue'
import { useUserAccountStore } from '@/modules/UserAccount/store'

const userAccountStore = useUserAccountStore()
const router = useRouter()

const goToProjects = () => {
  router.push({ name: 'ProjectList' })
}

const goToUserManagement = () => {
  router.push({ name: 'UserManagement' })
}
</script>

<style scoped>
.home-page {
  background-color: transparent;
}
.text-primary {
  color: var(--el-color-primary);
}
</style>
