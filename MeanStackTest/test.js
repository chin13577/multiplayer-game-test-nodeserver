let arr = [1, 2, 3, 4, 5];
let b = arr.forEach((value,index,array)=>{
    value=value+1;
    console.log(array);
    return value;
});
console.log({ b});

let arr1 = [1, 2, 3, 4, 5];
let a = arr1.map((value,index,array)=>{
    value = value+1;
    console.log(value);
    return value;
});
console.log(a);
var array1 = [1, 4, 9, 16];

// pass a function to map
const map1 = array1.map(x => x * 2);

console.log(map1);
