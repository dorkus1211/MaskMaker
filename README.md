MaskMaker
=========
以下のAssetにUIを追加し、使いやすいよう改良したアセットです。<br>
https://github.com/keijiro/unity-alphamask

Textureを選択し、一括でMask画像を生成できます

使い方
=========
1. Projectビューでマスク画像を生成したいTextureを選択します。 
2. MaskMaker->OpenTheMaskMakerでMaskMakerを表示します。
3. MaskMakerのTextures欄に選択したTextureが表示されます。
4. CreateでMask画像の生成を行います。<br>

・Assetsフォルダ内にMaskMaker.csを追加すると使えます。
・生成されるパスは元画像と同じ階層です。<br>
・Mask画像のファイル名は元画像の「.png」を「_mask.asset」に置き換えた名前になります。
・元画像と、マスク画像は自動的にプラットフォーム(Android,iPhoneのみ)に対応した圧縮形式に変換されます。<br>
  Android -> ETC_RGB4<br>
  iPhone -> PVRTC_RGB4<br>
・SwitchPlatformでプラットフォームを変更しても圧縮形式は変換されないため、再度このアセットで変換を行って下さい。
