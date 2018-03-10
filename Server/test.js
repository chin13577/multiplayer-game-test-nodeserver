const timeoutObj = setTimeout(() => {
    console.log('timeout beyond time');
    clearInterval(intervalObj);
  }, 3000);
  console.log("before");
  const immediateObj = setImmediate(() => {
    console.log('immediately executing immediate');
  });
  console.log('after');
  const intervalObj = setInterval(() => {
    console.log('interviewing the interval');
  }, 100);
  
//   clearTimeout(timeoutObj);
//   clearImmediate(immediateObj);
//   clearInterval(intervalObj);