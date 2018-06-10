#!/bin/bash

# Creates a dist package for selected runtime and plugins

RUNTIMES=( \
"win7-x64" \
#"linux-x64" \
)

PLUGINS=( \
"DefaultTheme" \
"IdentityBase.EntityFramework.InMemory" \
"IdentityBase.EntityFramework.MySql" \
"IdentityBase.EntityFramework.Npgsql" \
"IdentityBase.EntityFramework.SqlServer" \
"IdentityBase.EntityFramework.zDbInitializer" \
"IdentityBase.SendGrid" \
"IdentityBase.Smtp" \
"IdentityBase.Twilio"
)

for runtime in "${RUNTIMES[@]}"
do

    output=publish/$runtime/lib

    # Remove the old files
    rm -rf ./$output

    # Publish the host application
    dotnet publish ./src/IdentityBase.Web/IdentityBase.Web.csproj \
        -c Release -r $runtime -o ../../$output

    # Publish all the plugins
    for plugin in "${PLUGINS[@]}"
    do

        # Publish the plugin
        dotnet publish ./src/IdentityBase.Web/Plugins/$plugin/$plugin.csproj \
            -c Release -r $runtime -o \
            ../../../../$output/Plugins/$plugin

        # Remove assemblies from plugin folder which contains in host folder
        for file in ./$output/*
        do

            filename=`basename $file`

            [[ -e "./$output/Plugins/$plugin/$filename" ]] \
                 && rm -rf "./$output/Plugins/$plugin/$filename"
        done
    done
done

# Windows specific stuff

# Linux specific stuff