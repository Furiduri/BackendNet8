import request from '@/utils/request'

export function createUser(data: { username: string, email: string, password: string }) {
    return request({
        url: 'Users',
        method: 'post',
        data: {
            UserName: data.username,
            Email: data.email,
            Password: data.password
        }
    })
}

export function getUserList(params = {}) {
    return request({
        url: 'Users',
        method: 'get',
        params
    })
}
