<lane orientation="vertical" horizontal-content-alignment="middle">
    <banner background={@Mods/StardewUI/Sprites/BannerBackground} 
            background-border-thickness="48,0" padding="12"
            text={:HeaderText}
    />
    <frame layout="1280px 640px" margin="0,16,0,0" padding="32,24" background={@Mods/StardewUI/Sprites/ControlBorder}>
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
            <image *case="4" layout="stretch stretch" sprite={@Item/(O)74} /> 
        </frame>
        <scrollable peeking="128">
        <lane orientation="vertical">
            <grid layout="stretch content" item-layout="length: 144" item-spacing="32,32"
                                                  horizontal-item-alignment="middle">
                               
                
                <lane *repeat={:Buildings} orientation="vertical" horizontal-content-alignment="middle"
                                                       focusable="true">
                                <include name="Mods/ShedMonitor/Views/BuildingView" *context={:this} />
                                </lane>                   
            </grid>
            <grid layout="stretch content" item-layout="length: 96" item-spacing="16,16"
                horizontal-item-alignment="middle">
                <lane *repeat={ChestsSorted} orientation="vertical" horizontal-content-alignment="middle"
                      tooltip={:DisplayName} click=|Open()| focusable="true">

                <include name="Mods/ShedMonitor/Views/ChestDisplayView" *context={:this}/>
                    
                </lane>
            </grid>
         </lane>
        </scrollable>
        </panel>
    </frame>
</lane>