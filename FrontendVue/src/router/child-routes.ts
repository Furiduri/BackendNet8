
const importModule = import.meta.glob('../modules/**/*.vue')
const Layout = () => import('@/components/Layout/index.vue')
const LayoutView = () => import('@/components/Layout/LayoutView.vue')
const LayoutArea = () => import('@/components/Layout/LayoutArea.vue')

const childrenRoutes: Array<RouteRecordRaw> = [
  {
    path: 'test-layout',
    name: '布局测试',
    meta: {
      title: 'TestLayout'
    },
    component: LayoutArea
  },
  {
    path: 'home',
    name: 'Home',
    component: importModule['../modules/Home/pages/Index.vue'],
    meta: {
      title: 'Inicio'
    }
  },
  {
    path: 'user',
    component: Layout,
    name: 'User',
    meta: {
      title: '账户'
    },
    redirect: {
      name: 'UserLogin'
    },
    children: [
      {
        path: 'login',
        name: 'UserLogin',
        component: importModule['../modules/UserAccount/pages/login.vue'],
        meta: {
          title: '登录'
        }
      }
    ]
  },
  {
    path: 'project',
    component: Layout,
    name: 'Project',
    redirect: {
      name: 'ProjectList'
    },
    children: [
      {
        path: '',
        name: 'ProjectList',
        meta: {
          title: '项目列表'
        },
        component: importModule['../modules/Project/pages/list.vue']
      },
      {
        path: 'list',
        name: 'ProjectListAdmin',
        component: importModule['../modules/Project/pages/list.vue'],
        meta: {
          title: '项目管理'
        }
      }
    ]
  },
  {
    path: 'result',
    redirect: {
      name: 'ProjectList'
    }
  },
  {
    path: 'result/:projectId',
    component: LayoutView,
    name: 'result',
    redirect: {
      name: 'ResultOverview'
    },
    children: [
      {
        path: 'overview',
        name: 'ResultOverview',
        component: importModule['../modules/Result/pages/overview.vue'],
        meta: {
          title: '总览'
        }
      }
    ]
  },
  {
    path: 'user-management',
    component: Layout,
    name: 'UserManagement',
    meta: {
      title: 'Gestión de Usuarios',
      roles: ['Admin', 'Developer']
    },
    redirect: {
      name: 'UserCreate'
    },
    children: [
      {
        path: 'create',
        name: 'UserCreate',
        component: importModule['../modules/UserManagement/pages/CreateUser.vue'],
        meta: {
          title: 'Crear Usuario',
          roles: ['Admin', 'Developer']
        }
      }
    ]
  }
]

export default childrenRoutes
