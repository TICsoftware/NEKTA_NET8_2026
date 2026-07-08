
//All H2 Animation

document.querySelectorAll(".text-reveal").forEach((el)=>{

const text = el.textContent.trim();

el.innerHTML = "";

text.split("").forEach((letter,index)=>{

const span=document.createElement("span");

span.classList.add("char");

span.innerHTML=
letter===" "
? "&nbsp;"
: letter;

span.style.animationDelay=
`${index*0.05}s`;

el.appendChild(span);

});


const observer =
new IntersectionObserver((entries)=>{

entries.forEach((entry)=>{

if(entry.isIntersecting){

entry.target.classList.remove("active");

/* restart animation */
setTimeout(()=>{
entry.target.classList.add("active");
},50);

}

/* scroll up â†’ reset */
else{

entry.target.classList.remove("active");

}

});

},{
threshold:0.5
});

observer.observe(el);

});