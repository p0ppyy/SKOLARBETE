var open = false;


function openMenu() {
  if(!open){
    $(".dropdown").slideDown("slow");
    open = true;
  }
  else {
    $(".dropdown").slideUp("slow");
    open = false;
  }

}

function closeMenu() {
  $(".dropdown").slideUp("slow");
}
