import CryptoJS from 'crypto-js'

export const stringHelper = {
  md5hash
}

function md5hash(text: string) {
  return CryptoJS.MD5(text).toString().valueOf()
}
