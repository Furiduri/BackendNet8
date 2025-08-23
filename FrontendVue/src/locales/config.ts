import elementEnLocale from 'element-plus/es/locale/lang/en'
import elementZhLocale from 'element-plus/es/locale/lang/zh-cn'
import elementEsLocale from 'element-plus/es/locale/lang/es'

/**
 * 缺省值 i18n 语言
 */
export const DEFAULT_LANG = 'es-mx'

export const localesMapping = [
  {
    localeCode: 'zh-hans',
    localeName: '简体中文',
    elementLocale: elementZhLocale
  },
  {
    localeCode: 'en',
    localeName: 'English',
    elementLocale: elementEnLocale
  },
  {
    localeCode: 'es-mx',
    localeName: 'Español (MX)',
    elementLocale: elementEsLocale
  }
]

export const findLocaleByCode = (targetLocaleCode) => {
  return localesMapping.find(
    localeItem => localeItem.localeCode === targetLocaleCode
  )
}
