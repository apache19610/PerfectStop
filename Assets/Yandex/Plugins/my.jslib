mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
  },

  ShowAdv: function()
  {
YaGames.init().then(ysdk => ysdk.adv.showFullscreenAdv({
    callbacks: {
    	onOpen: () => {
          console.log('Video ad open.');
          myGameInstance.SendMessage("GameController", "OpenAdv");
        },
        onClose: function(wasShown) {
      	  console.log('Video ad closed.');
          myGameInstance.SendMessage("GameController", "CloseAdv");

        },
        onError: function(error) {
          // some action on error
        }
    }
}))

  },

  AddCoinsExtern: function(){
YaGames.init().then(ysdk => ysdk.adv.showRewardedVideo({
    callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
          myGameInstance.SendMessage("GameController", "OpenAdv");
        },
        onRewarded: () => {
          
        },
        onClose: () => {
          console.log('Video ad closed.');
          myGameInstance.SendMessage("GameController", "CloseAdv");
          myGameInstance.SendMessage("GameController", "BoznagrazdenieZaProsmotrReklami");
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
}))
  },
});