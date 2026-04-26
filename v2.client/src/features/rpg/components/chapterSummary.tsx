import React from "react";
import { Accordion, AccordionDetails, AccordionSummary, Box, Typography } from "@mui/material";
import ArrowDownwardIcon from "@mui/icons-material/ArrowDownward";
import { t } from "i18next";

import { Chapter, Hero, HeroDto, Place, SessionDto } from "@/features/rpg";
import HeroForm from "./heroForm";
import SessionForm from "./sessionForm";

export type ChapterTableProps = {
  chapter: Chapter;
};

export const ChapterSummary: React.FC<ChapterTableProps> = ({ chapter }) => {

  // Convert Place to SessionDto safely
  const convertToSessionDto = (place: Place): SessionDto => place as unknown as SessionDto;
    
  const toDto = (data: Hero): HeroDto => {
        return {...data, playerData: data.playerData || '', skills: data.skills || []} as unknown as HeroDto;
    }

  return (
    <Accordion>
      <AccordionSummary
        expandIcon={<ArrowDownwardIcon />}
        aria-controls="chapter-panel-content"
        id="chapter-panel-header"
      >
        <Typography>{chapter.title}</Typography>
      </AccordionSummary>

      <AccordionDetails>
        <Typography variant="body1" gutterBottom>
          {chapter.description}
        </Typography>

        {/* Heroes Section */}
        <Accordion>
          <AccordionSummary
            expandIcon={<ArrowDownwardIcon />}
            aria-controls="heroes-panel-content"
            id="heroes-panel-header"
          >
            <Typography>{t("rpg.story.heroes")}</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <Box>
              {chapter.heroes?.map((hero: Hero) => (
                <Accordion key={hero.id}>
                  <AccordionSummary
                    expandIcon={<ArrowDownwardIcon />}
                    aria-controls={`hero-${hero.id}-content`}
                    id={`hero-${hero.id}-header`}
                  >
                    <Typography>{`${hero.firstName} ${hero.lastName}`}</Typography>
                  </AccordionSummary>
                  <AccordionDetails>
                    <HeroForm hero={toDto(hero)} />
                  </AccordionDetails>
                </Accordion>
              ))}
            </Box>
          </AccordionDetails>
        </Accordion>

        {/* Places Section */}
        <Accordion>
          <AccordionSummary
            expandIcon={<ArrowDownwardIcon />}
            aria-controls="places-panel-content"
            id="places-panel-header"
          >
            <Typography>{t("rpg.story.places")}</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <Box>
              {chapter.places?.map((place: Place) => (
                <Accordion key={place.id}>
                  <AccordionSummary
                    expandIcon={<ArrowDownwardIcon />}
                    aria-controls={`place-${place.id}-content`}
                    id={`place-${place.id}-header`}
                  >
                    <Typography>{place.title}</Typography>
                  </AccordionSummary>
                  <AccordionDetails>
                    <SessionForm data={convertToSessionDto(place)} />
                  </AccordionDetails>
                </Accordion>
              ))}
            </Box>
          </AccordionDetails>
        </Accordion>
      </AccordionDetails>
    </Accordion>
  );
};