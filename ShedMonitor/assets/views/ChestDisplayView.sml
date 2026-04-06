<lane orientation="vertical"  horizontal-content-alignment="middle">
<panel layout="64px 128px" *context={:ChestSprite}>
    <image layout="stretch content" sprite={:Body} tint={:Tint} />
    <image layout="stretch content" sprite={:Overlay} *if={:DrawOverlay} tint={:OverlayTint}/>
    <image layout="stretch content" sprite={:Trim} />
</panel>
<frame layout="64px 64px" *context={:ItemSprite}
    background={@Mods/StardewUI/Sprites/ControlBorder} padding="16"
    horizontal-content-alignment="middle"
    vertical-content-alignment="middle">
    <image layout="content stretch" sprite={:Item}/>
</frame>
</lane>