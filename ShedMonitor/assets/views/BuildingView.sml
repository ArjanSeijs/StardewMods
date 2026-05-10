<lane orientation="vertical"  horizontal-content-alignment="middle">
<panel>
<frame layout="128px 192px"
    background={@Mods/StardewUI/Sprites/ControlBorder} padding="16"
    horizontal-content-alignment="middle"
    vertical-content-alignment="middle">
    <image layout="stretch content" sprite={:BuildingSprite} click=|Open()| />
</frame>
<frame layout="48px 48px"
       margin="0,0,0,0"
       padding="16,16" 
       background={@Mods/StardewUI/Sprites/ControlBorder}
>
    <image layout="stretch content" sprite={:ItemSprite} />
    </frame>
</panel>
</lane>