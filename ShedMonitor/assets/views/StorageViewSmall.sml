<frame>
<lane orientation="horizontal" horizontal-content-alignment="start" vertical-content-alignment="end">
    <frame layout="1280px 112px" margin="0,16,0,0" padding="32,24" background={@Mods/StardewUI/Sprites/ControlBorder}>
        <panel>
        <frame  layout="32px 32px"
                margin="0,0,0,0"
                padding="16,16" 
                background={@Mods/StardewUI/Sprites/ControlBorder}
                *float="before"
                *switch={Sort}
                click=|ToggleSort()|>
            <image *case="0" layout="stretch stretch" sprite={@Mods/StardewUI/Sprites/SmallUpArrow}/>
            <image *case="1" layout="stretch stretch" sprite={@Mods/StardewUI/Sprites/SmallRightArrow} />  
            <image *case="2" layout="stretch stretch" sprite={@Mods/StardewUI/Sprites/SmallDownArrow} />  
            <image *case="3" layout="stretch stretch" sprite={@Mods/StardewUI/Sprites/SmallLeftArrow} />  
        </frame>
        <scrollable peeking="128">
            <grid layout="stretch content" item-layout="length: 96" item-spacing="16,16"
                horizontal-item-alignment="middle">
                <lane *repeat={ChestsSorted} orientation="vertical" horizontal-content-alignment="middle"
                      tooltip={:DisplayName} click=|Open()| focusable="true">

                <include name="Mods/ShedMonitor/Views/ChestDisplayView" *context={:this} />
                    
                    
                </lane>
            </grid>
        </scrollable>
        </panel>
    </frame>
</lane>
</frame>