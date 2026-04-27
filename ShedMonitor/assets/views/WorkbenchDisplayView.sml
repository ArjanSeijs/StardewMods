<lane orientation="vertical"  horizontal-content-alignment="middle">
<panel layout="64px 128px">
    <image layout="stretch content" sprite={:WorkbenchSprite} />
</panel>
<frame layout="64px 64px" *context={:ItemSprite}
    background={@Mods/StardewUI/Sprites/ControlBorder} padding="16"
    horizontal-content-alignment="middle"
    vertical-content-alignment="middle">
    <image layout="content stretch" sprite={:Item}/>
</frame>
</lane>