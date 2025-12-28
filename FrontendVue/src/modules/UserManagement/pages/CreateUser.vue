<template>
  <div class="user-management-create p-20px">
    <el-card class="max-w-600px m-auto">
      <template #header>
        <div class="card-header">
          <span class="text-xl font-bold">Crear Nuevo Usuario</span>
        </div>
      </template>

      <el-form
        ref="userFormRef"
        :model="userForm"
        :rules="rules"
        label-width="120px"
        class="mt-4"
      >
        <el-form-item label="Username" prop="username">
          <el-input v-model="userForm.username" placeholder="Ingrese nombre de usuario" />
        </el-form-item>

        <el-form-item label="Email" prop="email">
          <el-input v-model="userForm.email" type="email" placeholder="Ingrese correo electrónico" />
        </el-form-item>

        <el-form-item label="Password" prop="password">
          <el-input v-model="userForm.password" type="password" show-password placeholder="Ingrese contraseña" />
        </el-form-item>

        <el-form-item>
          <el-button type="primary" :loading="loading" @click="handleCreate(userFormRef)">
            Crear Usuario
          </el-button>
          <el-button @click="resetForm(userFormRef)">Reiniciar</el-button>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import type { FormInstance, FormRules } from 'element-plus'
import { ElMessage } from 'element-plus'
import { createUser } from '../api'

const loading = ref(false)
const userFormRef = ref<FormInstance>()

const userForm = reactive({
  username: '',
  email: '',
  password: ''
})

const rules = reactive<FormRules>({
  username: [
    { required: true, message: 'El nombre de usuario es obligatorio', trigger: 'blur' },
    { min: 3, message: 'Mínimo 3 caracteres', trigger: 'blur' }
  ],
  email: [
    { required: true, message: 'El correo es obligatorio', trigger: 'blur' },
    { type: 'email', message: 'Formato de correo inválido', trigger: 'blur' }
  ],
  password: [
    { required: true, message: 'La contraseña es obligatoria', trigger: 'blur' },
    { min: 8, message: 'Mínimo 8 caracteres', trigger: 'blur' }
  ]
})

const handleCreate = async (formEl: FormInstance | undefined) => {
  if (!formEl) return

  await formEl.validate(async (valid) => {
    if (valid) {
      loading.value = true
      try {
        const { error, msg } = await createUser(userForm)
        if (!error) {
          ElMessage.success('Usuario creado exitosamente')
          resetForm(formEl)
        } else {
          ElMessage.error(msg || 'Error al crear usuario')
        }
      } catch (err: any) {
        ElMessage.error(err.message || 'Error de conexión')
      } finally {
        loading.value = false
      }
    }
  })
}

const resetForm = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  formEl.resetFields()
}
</script>

<style scoped>
.user-management-create {
  background-color: transparent;
}
</style>
