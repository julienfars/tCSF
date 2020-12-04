presets <- read.csv("presets.csv", sep=";", dec=",")
modulation <- 1:4
phase <- 5:8
zero <- rep(0,4)

setwd("Untersuchungen")

dname<-paste("_O([[:upper:]]{1})_201([[:digit:]]{1})-([[:digit:]]{2})-([[:digit:]]{2})_([[:alnum:]]*)\\.txt",sep="")

dateien<-list.files(path=".",pattern=dname)

s_u<-NA; s_d<-NA;
ergebnis<-c()

for (i in dateien) {

  aktTab<-read.table(i,skip=4,sep=";",dec=",",fill=T)
  Kontrast100<-match(100,aktTab[aktTab$V1=="Kontrast SC1",])
  Fr<-aktTab[aktTab$V1=="Frequenz",Kontrast100]
  if(Kontrast100>6) { Kontrast100<-Kontrast100-5 }
  
  typ<-NA
  
  kontraste <- aktTab[aktTab[,1]=="Kontrast SC1",c(3:6,8:11)]
  if (sum(kontraste[1:4]) == 0) { kontraste <- kontraste[5:8] } else { kontraste <- kontraste[1:4] }
  for (rt in names(presets)[-1]) { 
    if (sum(abs(5*presets[modulation,rt]-kontraste)) < .7) { typ <- rt}
  }
  
  if (sum(aktTab$V1=="Down: Schwelle erreicht!") == 0) { s_d<-100 }
  else {
    s_d<-(aktTab[aktTab$V1=="Down: Schwelle erreicht!",][[Kontrast100]]) 
  }
  
  if (sum(aktTab$V1=="Up: Schwelle erreicht!") == 0) { s_u<-100 }
  else { 
    s_u<-(aktTab[aktTab$V1=="Up: Schwelle erreicht!",][[Kontrast100]]) 
  }

  ergebnis<-rbind(ergebnis,data.frame(Dateiname=i,Typ=typ,Frequenz=as.integer(Fr),Down=as.double(s_d),Up=as.double(s_u)))
  
}

erg<-ergebnis[with(ergebnis,order(Typ,Frequenz)),]

farben <- c(L="red",M="green",S="blue",R="black",LmM="orange",LMSR="gray")

bmp("plot.bmp",width=480,height=480)
zeige <- unique(erg$Typ)
zeige <- c("L","M","S","R")
firstplot=T
for (rt in zeige) {
  farbe <- farben[rt]
  if (is.na(farbe)) { farbe <- "black" }
  if (firstplot) { 
    with(erg[erg$Typ==rt,],plot(Frequenz,200/(Up+Down)/presets[9,rt],log="xy",xlab="Frequenz",ylab="Empfindlichkeit",xlim=c(1,44),ylim=c(1,500),type="b",lty=presets[10,rt],col=farbe))
    firstplot=F
  }
  else { 
    with(erg[erg$Typ==rt,],points(Frequenz,200/(Up+Down)/presets[9,rt],type="b",lty=presets[10,rt],col=farbe)) 
  }
}
dev.off();

erg<-cbind(erg,Threshold=c(0),Sensitivity=c(0))
for (i in 1:length(erg$Frequenz)) {
  K <- presets[9,as.character(erg$Typ[i])]
  erg$Threshold[i]<-K*(erg$Up[i]+erg$Down[i])/2
  erg$Sensitivity[i]<-100/erg$Threshold[i]
}

write.table(erg,"daten.csv",dec=",",sep=";")

