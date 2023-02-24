import React from 'react'
import Box from '@mui/material/Box'
import { Divider, Typography } from '@mui/material'
import Stack from '@mui/material/Stack'
import Avatar from '@mui/material/Avatar'
import Paper from '@mui/material/Paper'
import Carrdds from './Carrdds'
import ThumbUpIcon from '@mui/icons-material/ThumbUp'
import InsertCommentIcon from '@mui/icons-material/InsertComment'
import { Button } from '@mui/material'
import TextField from '@mui/material/TextField'
import MessageIcon from '@mui/icons-material/Message'


function HelpFaq() {
  return (
    <div>
      {/* Main Box  */}
      <Box sx={{ display: 'flex', flexDirection: 'row' }}>
        {/* leftSide box */}
        <Box sx={{ height: '364vh', width: '21rem' }}>
          MainBox1
          {/* Content before box content*/}
          <Box>
            {/* Captcha box */}
            <Box
              sx={{
                height: '13vh',
                width: '17rem',
                border: '1px solid black',
                ml: 5,
                mt: 5,
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center'
              }}
            >
              <Typography>hCaptcha</Typography>
            </Box>
            <Button variant='outlined' sx={{ mt: 5, ml: 5 }}>
              Submit
            </Button>

            <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-around', margin: '10px' }}>
              <Button variant='contained' size='small'>
                Click here to post
              </Button>
              <Button variant='contained' size='small'>
                Capture
              </Button>
            </Box>

            <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'space-between', mt: 5, ml: 5 }}>
              <TextField
                id='filled-search'
                type='Search field'
                placeholder='Type Something'
                variant='filled'
              />
              <Button variant='contained' >Search</Button>
            </Box>
            <br />

            {/* cards box1 CVC */}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> cvc </Typography>
              </Box>
            </Box>
            <Divider />

            {/* cards box2 CVC */}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />{' '}
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> cvc </Typography>
              </Box>
            </Box>
            <Divider />

            {/* cards box3 CVC*/}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> cvc </Typography>
              </Box>
            </Box>
            <Divider />

            {/* cards box4 dfdf*/}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> dfdf </Typography>
              </Box>
            </Box>
            <Divider />

            {/* cards box5 sd*/}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> sd </Typography>
              </Box>
            </Box>
            <Divider />

            {/* cards box6 dsd*/}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> dsd </Typography>
              </Box>
            </Box>
            <Divider />

            {/* cards box7 Sumbul testing 1*/}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> Sumbul testing 1 </Typography>
              </Box>
            </Box>
            <Divider />

            {/* cards box8 help pdf*/}
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  height: '27vh',
                  width: '15rem',
                  border: '1px solid black',
                  ml: 8,
                  mt: 5,
                  display: 'flex',
                  justifyContent: 'center',
                  alignItems: 'center'
                }}
              >
                <Typography>
                  <MessageIcon />
                </Typography>
              </Box>
              <Box marginLeft={7} marginTop={2}>
                <Typography> Help pdf </Typography>
              </Box>
            </Box>
          </Box>
          <br />
        </Box>

        {/* rightSide box  */}
        <Box elevation={1} sx={{ height: '364vh', width: '45rem', overflow: 'scroll' }}>
          {/* first box in main box Help pdf*/}
          <Box
            sx={{
              height: '66vh',
              width: '44rem',
              border: '1px solid black',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To</b>
                </Typography>
                <Typography fontSize='12px'>August 26 2022 at 3:20 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>Help pdf</Typography>
            </Box>

            {/* pdf box  */}
            <Box sx={{ m: 2 }}>
              <Paper elevation={1} sx={{ height: '33vh', width: '44vw', overflow: 'scroll' }}>
                <Carrdds />
                <br />
                <Carrdds />
              </Paper>
            </Box>

            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>1-Likes 0-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Liked
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />
          </Box>
          <br />

          {/* second box in main box Sumbul testing 1 */}
          <Box
            sx={{
              height: '35vh',
              width: '44rem',
              border: '1px solid black',
              // backgroundColor: 'aliceblue',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To</b>
                </Typography>
                <Typography fontSize='12px'>August 26 2022 at 3:13 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>Sumbul testing 1</Typography>
            </Box>
            <br />
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>0-Likes 0-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Like
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />
          </Box>
          <br />

          {/* third box in main box  sd*/}
          <Box
            sx={{
              height: '58vh',
              width: '44rem',
              border: '1px solid black',
              // backgroundColor: 'aliceblue',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To Self</b>
                </Typography>
                <Typography fontSize='12px'>August 18 2022 at 1:19 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>sd</Typography>
            </Box>
            <br />
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>1-Likes 1-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Liked
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />

            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 10 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To Self</b>
                </Typography>
                <Typography fontSize='12px'>2022-Aug-25 04:39:01</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>nddfj</Typography>
            </Box>
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>Like · Reply · Edit · Delete</Typography>
            </Box>
          </Box>
          <br />

          {/* fourth box in main box dsd */}
          <Box
            sx={{
              height: '35vh',
              width: '44rem',
              border: '1px solid black',
              // backgroundColor: 'aliceblue',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To Self</b>
                </Typography>
                <Typography fontSize='12px'>August 18 2022 at 1:16 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>dsd</Typography>
            </Box>
            <br />
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>0-Likes 0-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Like
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />
          </Box>
          <br />

          {/* fifth box in main box   cvc*/}
          <Box
            sx={{
              height: '35vh',
              width: '44rem',
              border: '1px solid black',
              // backgroundColor: 'aliceblue',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To Self</b>
                </Typography>
                <Typography fontSize='12px'>August 17 2022 at 2:28 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>cvc</Typography>
            </Box>
            <br />
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>0-Likes 0-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Like
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />
          </Box>
          <br />

          {/* sixth box in main box  cvc*/}
          <Box
            sx={{
              height: '35vh',
              width: '44rem',
              border: '1px solid black',
              // backgroundColor: 'aliceblue',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To Self</b>
                </Typography>
                <Typography fontSize='12px'>August 17 2022 at 2:26 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>cvc</Typography>
            </Box>
            <br />
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>0-Likes 0-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Like
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />
          </Box>
          <br />

          {/* seventh box in main box  cvc */}
          <Box
            sx={{
              height: '35vh',
              width: '44rem',
              border: '1px solid black',
              // backgroundColor: 'aliceblue',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To Self</b>
                </Typography>
                <Typography fontSize='12px'>August 17 2022 at 2:24 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>cvc</Typography>
            </Box>
            <br />
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>1-Likes 0-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Liked
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />
          </Box>
          <br />

          {/* eighth box in main box dfdf */}
          <Box
            sx={{
              height: '35vh',
              width: '44rem',
              border: '1px solid black',
              // backgroundColor: 'aliceblue',
              ml: 1,
              mt: 1
            }}
          >
            {/* image/adminitrator/date */}
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
              {/* image box  */}
              <Box>
                <Stack direction='row'>
                  <Avatar
                    alt='Remy Sharp'
                    src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                  />
                </Stack>
              </Box>
              <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                <Typography fontSize='12px'>
                  <b> Administrator To Self</b>
                </Typography>
                <Typography fontSize='12px'>August 17 2022 at 2:23 PM</Typography>
              </Box>
            </Box>
            <Box sx={{ ml: 4, mt: 2 }}>
              <Typography>dfdf</Typography>
            </Box>
            <br />
            <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Typography>1-Likes 0-Comments</Typography>
            </Box>
            <Divider />
            <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
              <Box>
                <span>
                  <ThumbUpIcon />
                  Liked
                </span>{' '}
                <spacing />
                <span>
                  <InsertCommentIcon />
                  Comment
                </span>
              </Box>
            </Box>
            <Divider />
          </Box>
          <br />
        </Box>
      </Box>
    </div>
  )
}

export default HelpFaq
