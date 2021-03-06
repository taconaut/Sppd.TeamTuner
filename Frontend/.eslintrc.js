module.exports = {
  root: true,
  env: {
    node: true
  },
  'extends': [
    'plugin:vue/essential',
    '@vue/standard',
    '@vue/typescript'
  ],
  rules: {
    'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
    'space-before-function-paren': [
      'warn',
      {
        anonymous: 'ignore',
        named: 'ignore',
        asyncArrow: 'ignore'
      }
    ]
  },
  parserOptions: {
    parser: '@typescript-eslint/parser'
  }
}
