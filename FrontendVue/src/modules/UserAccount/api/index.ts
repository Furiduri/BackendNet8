import { IResponse, IUser } from '@/interfaces/IAll'
import { DEFAULT_LANG } from '@/locales/config'
import request from '@/utils/request'
import { toBase64Unicode } from '@/utils/stringUtils'

export function login(data: { username: string, password: string }): Promise<IResponse<{
  language: string;
  token: string;
  user: IUser;
}>> {
  data.password = toBase64Unicode(data.password)
  return request({
    url: 'Auth/login',
    method: 'post',
    data: {
      username: data.username,
      password: data.password
    }
  })
}

export function logout() {
  return request({
    url: '/logout',
    method: 'post'
  })
}

export function getUserInfoData(params = {}) {
  return request({
    url: 'Auth/user_info',
    method: 'get',
    params
  })
}

export function updateChangeLanguage(data) {
  return request({
    url: '/acl/changelanguage',
    method: 'post',
    data
  })
}

export function getDemoTestList(params) {
  return request({
    url: '/api/demo_test/list',
    method: 'get',
    params
  })
}

export function createDemoTest(data) {
  return request({
    url: '/api/demo_test',
    method: 'post',
    data
  })
}

export function updateDemoTest(data) {
  return request({
    url: `/api/demo_test/${data.demoId}`,
    method: 'put',
    data
  })
}

export function deleteDemoTest(demoId) {
  return request({
    url: `/api/demo_test/${demoId}`,
    method: 'delete'
  })
}
