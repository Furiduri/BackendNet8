import { defineStore } from 'pinia'
import { store } from '@/store'

import { sleep } from '@/utils/request'
import {
  getUserInfoData,
  login,
  logout,
  updateChangeLanguage
} from '@/modules/UserAccount/api'
import { changeLocale } from '@/locales/useLocale'
import { DEFAULT_LANG } from '@/locales/config'

export interface IUserAccountState {
  locale: string
  demoList: any
  userInfo: any
}

export const useUserAccountStore = defineStore('UserAccount', {
  state: (): IUserAccountState => {
    return {
      locale: DEFAULT_LANG,
      demoList: {},
      userInfo: {}
    }
  },
  getters: {
    isAdmin(state) {
      const roles = state.userInfo?.roles || state.userInfo?.Roles
      return roles?.includes('Admin') || roles?.includes('Developer')
    },
    isDeveloper(state) {
      const roles = state.userInfo?.roles || state.userInfo?.Roles
      return roles?.includes('Developer')
    },
    isAdminOrDev(state) {
      const roles = state.userInfo?.roles || state.userInfo?.Roles
      return roles?.includes('Admin') || roles?.includes('Developer')
    }
  },
  actions: {
    async GetModuleTestList(params) {
      // TODO: 模拟响应时间
      await sleep(1000)
      // TODO: 模拟 api
      // const result = await getDemoTestList(params)
      const result = {
        test: 'ok'
      }
      this.demoList = result
      return result
    },
    async updateChangeLanguage(params) {
      const result = await updateChangeLanguage(params)
      return this.filterResponse(result)
    },
    setLanguage(data) {
      this.locale = data.locale
    },
    async login(data) {
      const res = await login({ username: data.username, password: data.password })
      return this.filterResponse(res, ({ data }) => {
        if (data && data.user) {
          // Normalizar datos para evitar problemas de casing
          this.userInfo = {
            username: data.user.userName || data.user.username,
            email: data.user.email || data.user.Email,
            userId: data.user.userId || data.user.UserId,
            roles: data.user.roles || data.user.Roles || []
          }
        }
      })
    },
    async logout() {
      const res = await logout()
      return this.filterResponse(res, null, () => { })
    },
    async getUserInfo() {
      const res = await getUserInfoData()
      return this.filterResponse(res, ({ data }) => {
        this.userInfo = data
      })
    }
  }
})

// Need to be used outside the setup
export function useUserAccountStoreWithOut() {
  return useUserAccountStore(store)
}
