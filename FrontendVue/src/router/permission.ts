import { useUserAccountStore } from '@/modules/UserAccount/store'
import { ElMessage } from 'element-plus'

import Cookie from 'js-cookie'
import { allowlist } from '@/router/auth-list'
import { systemTitle } from '@/locales/data'

import NProgress from 'nprogress'
import type { Router } from 'vue-router'
import { changeLocale } from '@/locales/useLocale'

NProgress.configure({
  showSpinner: false
})

export function createRouterGuards(router: Router) {
  router.beforeEach(async (to, from, next) => {
    const userAccountStore = useUserAccountStore()

    try {
      NProgress.start()

      document.title = `${to.meta.title || ''} - ${systemTitle}`

      console.log('😄😄😄 ', to)

      const currentRouteLocale = to.params.locale

      if (
        allowlist.find(
          name => to.name === name
        )
      ) {
        next()
        return
      }

      if (!Cookie.get('token')) {
        next(`/${currentRouteLocale || userAccountStore.locale}/user/login`)
        return
      }

      // Obtener información del usuario si no existe en el store
      const hasUserInfo = !!(userAccountStore.userInfo?.username || userAccountStore.userInfo?.userName)

      if (!hasUserInfo) {
        console.log('Fetching user info...')
        const { data, error } = await userAccountStore.getUserInfo()

        if (error) {
          console.error('Error fetching user info:', error)
          Cookie.remove('token')
          const _locale = await changeLocale(
            currentRouteLocale || userAccountStore.locale
          )
          // Redirigir al login si hay error al obtener info
          next(`/${_locale}/user/login`)
          return
        }

        if (data) {
          console.log('User info fetched successfully')
          await changeLocale(currentRouteLocale || data.language || userAccountStore.locale)
        }
      }

      // Role-based access control
      const requiredRoles = to.meta.roles as string[]
      if (requiredRoles && requiredRoles.length > 0) {
        const roles = userAccountStore.userInfo?.roles || userAccountStore.userInfo?.Roles
        const hasPermission = requiredRoles.some(role => roles?.includes(role))

        if (!hasPermission) {
          ElMessage.error('No tiene permisos para acceder a esta página')
          next(`/${currentRouteLocale || userAccountStore.locale}/project`)
          return
        }
      }

      await changeLocale(currentRouteLocale || userAccountStore.locale)
      next()
    } catch (error) {
      console.error('Permission Guard: Unexpected error', error)
      next()
    }
  })

  router.afterEach((to) => {
    NProgress.done()
  })
}
