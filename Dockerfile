# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /var/matrix/app/enter_the_matrix
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish --configuration Release
RUN cp -r /var/matrix/app/enter_the_matrix/bin/Release/netcoreapp3.1/* .
RUN apt-get update && apt-get install -y graphviz libgdiplus

ENTRYPOINT ["dotnet", "run", "Enter_The_Matrix.dll"]