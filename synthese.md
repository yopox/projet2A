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
    - \usepackage[utf8]{inputenc}
    - \fancyhead[L]{\leftmark}
    - \fancyhead[R]{Projet long}
    - \makeatletter
    - \renewcommand{\@chapapp}{Partie}
    - \makeatother
    - \usepackage[Bjornstrup]{fncychap}
---

# Introduction

## Présentation

### Concept

## Méthodologie

# Cahier des charges

# Programmation

## Principe général

### Framework `MonoGame`

Nous avons choisi de programmer en `C#` avec le framework `MonoGame`[^framework]. Il s'agit d'un framework open source avec une communauté d'utilisateurs et de développeurs très active. Plusieurs jeux 2D récents ont été codés avec ce framework (entre autres Celeste, Stardew Valley, Axiom Verge, Flinthook, Towerfall Ascention)[^showcase], ce qui nous a conforté dans notre choix.

[^framework]: Page d'accueil du framework : http://www.monogame.net/
[^showcase]: Jeux créés avec `MonoGame` mis en avant : http://www.monogame.net/showcase/?Featured

### États du jeu

## Acteurs

### Classe `Player`

## Affichage

### Classe `Camera`

### Fonction `Draw`

## Moteur physique

## Interactions

### Classe `MapObject`

## Autres systèmes

### Sauvegarde

### Cinématiques

# Conclusion

# Annexes
