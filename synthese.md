---
title: Jeu Vidéo - Synthèse
author: Descouens Nicolas, Vignier Louis
date: 2018/2019
lang: FR
institute: CentraleSupélec
documentclass: report
toc: true
numbersections: true
header-includes:
    - \usepackage{geometry}
    - \geometry{a4paper, margin=1.25in}
    - \usepackage{fancyhdr}
    - \pagestyle{fancy}
    - \usepackage{booktabs}
    - \usepackage{graphicx}
    - \usepackage{sistyle}
    - \usepackage{enumerate}
    - \usepackage[utf8]{inputenc}
    - \fancyhead[L]{\leftmark}
    - \fancyhead[R]{Projet long}
    - \makeatletter
    - \renewcommand{\@chapapp}{Partie}
    - \makeatother
    - \usepackage[Bjornstrup]{fncychap}
---

# Introduction

Ce projet est né de notre volonté de collaborer avec des étudiants de l'École Supérieure d'Art de Lorraine qui se situe à Metz. Cette école est liée à Supélec par une convention qui inclut la possibilité pour des élèves d'une école de prendre des cours dans l'autre --- dans les faits cette convention est uniquement utilisée pour un électif de première année du cursus Supélec. C'est cet électif qui a fait naître cette envie de collaboration.

La notion d'art dans le cursus Supélec est inexistante (mis à part dans l'électif cité précédemment), c'est pourquoi cette collaboration fait sens pour permettre de faire un projet pluridisciplinaire et abouti en s'éloignant d'un projet long "DIY".

Le jeu vidéo est une forme d'art qui mélange plusieurs disciplines : une partie visuelle avec des dessinateurs, une partie sonore avec des compositeurs, une partie narrative avec des écrivains, et une partie technique avec des programmeurs. Au cours de notre cursus nous avons été formé à la partie technique à travers les cours de génie logiciel et d'algorithmes. Les autres parties correspondent aux formations proposées par l'ÉSAL.

En moins d'un mois nous avons pu créer une équipe : quatre étudiants de l'ÉSAL ont été intéressés par notre proposition. L'équipe finale se compose de :

\begin{itemize}
    \item Élèves de CentraleSupélec:
    \begin{itemize}
        \item Nicolas Descouens
        \item Louis Vignier
    \end{itemize}
    \item Élèves de l'ÉSAL:
    \begin{itemize}
        \item Justine Allmang : Son
        \item Florian Ballié : Pixel Art
        \item Audrey Delay : Pixel Art
        \item Arthur Lambert : Scénario
    \end{itemize}
\end{itemize}

Nous avons été agréablement surpris de voir que le projet a suscité beaucoup d'intérêt à l'ÉSAL, chez l'encadrante Mme Bak, et chez les élèves : l'équipe d'artistes est finalement plus nombreuse que l'équipe d'élèves ingénieurs.

Dans la prochaine partie, nous détaillerons notre façon de travailler ainsi que les outils utilisés, puis nous détaillerons les points principaux du code.

# Organisation du travail

## Méthodologie

### Réunions

Pour présenter l'avancée de chacun sur le projet, nous nous sommes réunis à l'ÉSAL huit fois. Réunir tous les membres du projet a été difficile car les élèves artistes sont dans des années différentes ainsi que des cursus différents : tous les artistes n'étaient pas présents à chaque réunion.

Lors de la première réunion, nous avons établi un cahier des charges pour le projet. Mme Bak était présente lors de cette réunion et a d'abord dirigé la discussion sur ce que chacun de nous aimions pour faire ressortir l'aspect sensitif plutôt que l'aspect pragmatique (si on avait simplement parlé de nos compétences respectives par exemple). Il s'est trouvé que les différentes compétences de l'équipe se complétaient bien et couvraient les différents aspects d'un jeu vidéo.

Ces réunions ont été l'occasion de découvrir une nouvelle façon de penser et de parler car les artistes ont une approche projet différente des ingénieurs : on pourrait dire que les ingénieurs ont une façon très cadrée d'aborder un projet (en établissant un cahier des charges précis), alors que les artistes veulent explorer plein d'idées.

Pour rendre compte de l'avancée du projet, nous avons produit des compte-rendus de réunion à la fin de chaque réunion. Nous avons également fait une réunion avec M. Tavernier fin Décembre.

Cependant, le rythme des réunions (en moyenne une par mois) ne permet pas de se coordonner efficacement car il faut communiquer rapidement pour se mettre d'accord sur certains détails. C'est pourquoi nous avons mis en place un serveur Discord.

### Discord

Discord est une application de messagerie texuelle et vocale où les conversations sont regroupées en serveurs puis en salons. Nous avons donc créé un serveur dédié au projet et plusieurs salons pour séparer les conversations (général, graphismes, dev, son, scénario).

Tout le monde a adopté cet outil rapidement et nous l'avons beaucoup utilisé pour avoir un retour rapide sur quelque chose ou bien pour poser une question à un artiste.

### GitHub

Nous avons utilisé GitHub pour travailler à deux sur le code du jeu.

Nous avons rédigé un tutoriel sur Discord pour que les artistes puissent installer Git sur leur ordinateur et contribuer au projet sans avoir à nous envoyer de fichiers sur Discord. Justine et Florian se sont inscrits sur la plateforme mais uniquement Justine a réalisé des `commits` en ajoutant des sons.

## Cahier des charges

### Mise en place

Le cahier des charges du projet a été établi en plusieurs étapes puis redéfini au cours du projet.

Lors du démarrage du projet, nous avons fait valider un cahier des charges assez général dans le descriptif du projet en septembre 2018 :

\begin{enumerate}[i]
    \item Maîtriser des technologies récentes liées au jeu vidéo
    \item Se familiariser avec le travail en groupe au sein d'une équipe pluridisciplinaire
    \item Exploiter des connaissances de génie logiciel
\end{enumerate}

Nous avons parfaitement rempli ce cahier des charges dans le sens où nous avons produit un jeu jouable avec des artistes.
Une fois que l'équipe a été constituée, nous avons établi un second cahier des charges tous ensemble :

\begin{enumerate}[i]
    \item Créer un jeu en 2D
    \item Le gameplay est un jeu de plateforme avec une histoire
    \item Le jeu comporte un scénario avec plusieurs environnements et des boss
\end{enumerate}

Nous nous sommes rendus compte plus tard que ce cahier des charges était trop ambitieux : faire plusieurs environnements demande un travail graphique important, et les deux artistes en charge des graphismes ne pouvaient pas assumer cette charge de travail en parallèle de leurs cours à l'ÉSAL. En conséquence, nous avons redéfini les contours du projet le 19 Mars en adoptant le cahier des charges suivant :

\begin{enumerate}[i]
    \item Créer un jeu en 2D
    \item Le gameplay est un jeu de plateforme avec une histoire
    \item Le jeu comporte un scénario avec un seul environnement jouable
    \item Des cinématiques avec des fonds fixes permettent de simuler les autres environnements
\end{enumerate}

### Respect du cahier des charges

# Programmation

## Principe général

### Framework `MonoGame`

Nous avons choisi de programmer en `C#` avec le framework `MonoGame`. Il s'agit d'un framework open source avec une communauté d'utilisateurs et de développeurs très active. Plusieurs jeux 2D récents ont été codés avec ce framework (entre autres Celeste, Stardew Valley, Axiom Verge, Flinthook, Towerfall Ascention), ce qui nous a conforté dans notre choix.

De plus, nous avions envie d'apprendre un nouveau langage pour gagner de nouvelles compétences : le `C#` nous a alors paru un très bon choix étant donné qu'il s'agit d'un langage plutôt récent (2003) par rapport au `C++` (années 80) et au `Java` (années 90).

Nous avons choisi d'utiliser un framework pour avoir des fonctions qui nous permettent de faire les tâches de base dans le code d'un jeu tel qu'afficher une image à l'écran ou jouer un son par exemple. Ainsi, on a plus de temps pour se concentrer sur les aspects intéressants du projet comme les collisions ou la gestion des sources sonores.



### UML

Sur la figure \ref{uml}, on représente le diagramme UML simplifié (sans les attributs et fonctions des classes pour plus de lisibilité). Nous avons séparé le code en 5 packages :

\begin{itemize}
    \item States : Contient le code lié à la gestion des différents états du jeu (cinématique, phases de plateforme, menu pause, etc…)
    \item Definitions : Contient les informations liées aux ressources graphiques (taille, nom), ainsi que des fonctions utilitaires (calculs, lecture de scripts, etc…)
    \item Entities : Contient le code lié aux acteurs tels que le joueur
    \item RenderEngine : Contient le code lié à la gestion de l'affichage (affichage du décor, de l'arrière-plan, du joueur, etc…)
    \item PhysicsEngine : Contient le code lié à la physique (collisions, gravité)
\end{itemize}

![Diagramme UML simplifié des différentes classes du jeu\label{uml}](soutenance/assets/uml.png){ width=110% }

### Classe principale

`Game1` est la classe principale du projet : la fonction `Update` et la fonction `Draw` de `Game1` sont appelées 60 fois par seconde. 

# Conclusion

Travailler sur ce projet aura été très enrichissant, car cela nous a permis de nous ouvrir à de nouveaux fonctionnements que celui très cadré des ingénieurs. Travailler dans une équipe avec des profils variés a été indispensable pour arriver à un résultat unique.

Ce projet montre bien l'aspect "conception" car nous avons conçu l'architecture du jeu (une partie liée au moteur physique, graphique, aux états du jeu et aux définitions variées), et nous avons défini et redéfini les contours du projet avec les artistes.

Le travail en groupe a permis d'avoir un projet complet comportant des graphismes et des sons, mais nous a parfois ralenti dans notre travail : nous avons préféré attendre de recevoir les graphismes liés à une fonctionnalité avant de l'ajouter en jeu, ce qui nous a fait fortement dépendre du travail graphique.

# Annexes
