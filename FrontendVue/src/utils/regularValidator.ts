const validatorPositiveInteger = (rule, value, callback) => {
  if (/(^[1-9]\d*$)/.test(value)) {
    callback()
  } else {
    callback(new Error('Please enter a positive integer'))
  }
}

const validatorMaxCountFunction = (errorMsg = 'Maximum three', count = 3) => {
  return (rule, value, callback) => {
    if (!value || !value.length) {
      callback(new Error('Cannot be empty'))
    } else if (value.length > count) {
      callback(new Error(errorMsg))
    } else {
      callback()
    }
  }
}

export {
  validatorPositiveInteger,
  validatorMaxCountFunction
}
