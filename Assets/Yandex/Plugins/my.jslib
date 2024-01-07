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

          myGameInstance.SendMessage("Yandex", "OpenAdv");
        },
        onClose: function(wasShown) {
      	  console.log('Video ad closed.');
          myGameInstance.SendMessage("Yandex", "CloseAdv");

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
          myGameInstance.SendMessage("Yandex", "OpenAdv");
        },
        onRewarded: () => {
          console.log('Video onRewarded.');
         myGameInstance.SendMessage("GameController", "AddCoins");
        },
        onClose: () => {
          console.log('Video ad closed.');
          myGameInstance.SendMessage("Yandex", "CloseAdv");
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
    }
}))
  },
});